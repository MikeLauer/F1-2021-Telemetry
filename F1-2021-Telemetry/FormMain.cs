using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Codemasters.F1_2021;
using System.Timers;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;

namespace F1_2021_Telemetry
{
    /// <summary>
    /// Main form of the program showing the leaderboard table and the driver circle.
    /// 
    /// It contains a UDP Receiver for recieving data from the game.
    /// </summary>
    public partial class FormMain : Form
    {
        // Constants
        public static readonly Color GlobalColorGray = Color.FromArgb(50, 50, 50);
        public static readonly int NumberOfDrivers = 20;

        private UdpListener UdpListener; // Receiving data from the game

        private SessionPacket.SessionType CurrentSessionType;
        public SessionPacket SessionPacket;
        public LapPacket LapPacket;
        public ParticipantPacket ParticipantPacket;
        public CarStatusPacket CarStatusPacket;
        public CarDamagePacket CarDamagePacket;

        private LeaderboardManager LeaderboardManager; // leadboard table
        public Leaderboard LatestLeaderboard = null;

        private LapTimeGraph LapTimeGraph;

        private System.Timers.Timer UpdateTableTimer;
        private int UpdateFrequency = 30;

        private DriverCircle DriverCircle; // driver circle

        private System.Timers.Timer UnmarkPositionTimer; // unmark colored position changes
        private System.Timers.Timer TimeLeftBlinkTimer; // blink if there is little time left in the session
        private System.Timers.Timer SafetyCarBlinkTimer;

        private Label[] MarshalZoneLabels; // To store all labels and access them in a handy way
        private Label[] RainPercentageLabels;
        private Label[] WeatherForecastLabels;

        private FormSettings FormSettings; // Form for settings
        private bool SettingsOpen = false; // Flag whether settings are open or not

        public bool PerformanceMode = false;
        public bool HighlightOwnPlayer = true;
        public bool HighLightOtherPlayers = true;
        public bool ShowWeather = true;

        public FormMain()
        {
            InitializeComponent();

            LeaderboardManager = new LeaderboardManager();
            DriverCircle = new DriverCircle(this, pb_driverCircle);
            UdpListener = new UdpListener(this); // Create UDP listener

            UpdateTableTimer = new System.Timers.Timer();
            UpdateTableTimer.Interval = (int)(1000 / this.UpdateFrequency);
            UpdateTableTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateTable);

            this.LapTimeGraph = new LapTimeGraph(this, this.lapTimeChart);

            // Initialize leaderboard table
            for (int i = 0; i < NumberOfDrivers; i++)
            {
                dataGridView_leaderboard.Rows.Add(); // Add empty row

                // Color every second row gray
                if (i % 2 != 0)
                {
                    for (int k = 0; k < dataGridView_leaderboard.Rows[i].Cells.Count; k++)
                    {
                        dataGridView_leaderboard.Rows[i].Cells[k].Style.BackColor = GlobalColorGray;
                    }
                }
            }

            // Timer to unmark position change coloring
            UnmarkPositionTimer = new System.Timers.Timer();
            UnmarkPositionTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnUnmarkPositionChanges);
            UnmarkPositionTimer.Interval = 5000;

            // Timer to blink if little session time is left
            TimeLeftBlinkTimer = new System.Timers.Timer();
            TimeLeftBlinkTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimeLeftBlink);
            TimeLeftBlinkTimer.Interval = 500;

            // Timer to blink if (virtual) safety car is deployed
            SafetyCarBlinkTimer = new System.Timers.Timer();
            SafetyCarBlinkTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnSafetyCarBlink);
            SafetyCarBlinkTimer.Interval = 500;

            // Insert marshalzone labels in array for easy access later
            MarshalZoneLabels = new Label[21];
            MarshalZoneLabels[0] = label_Mz1;
            MarshalZoneLabels[1] = label_Mz2;
            MarshalZoneLabels[2] = label_Mz3;
            MarshalZoneLabels[3] = label_Mz4;
            MarshalZoneLabels[4] = label_Mz5;
            MarshalZoneLabels[5] = label_Mz6;
            MarshalZoneLabels[6] = label_Mz7;
            MarshalZoneLabels[7] = label_Mz8;
            MarshalZoneLabels[8] = label_Mz9;
            MarshalZoneLabels[9] = label_Mz10;
            MarshalZoneLabels[10] = label_Mz11;
            MarshalZoneLabels[11] = label_Mz12;
            MarshalZoneLabels[12] = label_Mz13;
            MarshalZoneLabels[13] = label_Mz14;
            MarshalZoneLabels[14] = label_Mz15;
            MarshalZoneLabels[15] = label_Mz16;
            MarshalZoneLabels[16] = label_Mz17;
            MarshalZoneLabels[17] = label_Mz18;
            MarshalZoneLabels[18] = label_Mz19;
            MarshalZoneLabels[19] = label_Mz20;
            MarshalZoneLabels[20] = label_Mz21;

            RainPercentageLabels = new Label[5];
            RainPercentageLabels[0] = label_rainPercentage0Min;
            RainPercentageLabels[1] = label_rainPercentage5Min;
            RainPercentageLabels[2] = label_rainPercentage10Min;
            RainPercentageLabels[3] = label_rainPercentage15Min;
            RainPercentageLabels[4] = label_rainPercentage30Min;

            WeatherForecastLabels = new Label[5];
            WeatherForecastLabels[0] = label_weatherForecast0Min;
            WeatherForecastLabels[1] = label_weatherForecast5Min;
            WeatherForecastLabels[2] = label_weatherForecast10Min;
            WeatherForecastLabels[3] = label_weatherForecast15Min;
            WeatherForecastLabels[4] = label_weatherForecast30Min;


            toolTip_performanceMode.SetToolTip(this.button_pitstopDeltaAuto, "Sets delta to recommended for current track.\nNot updated to F1-2021 !");


            // STARTING
            UdpListener.Start();
            UpdateTableTimer.Start();
        }

        public void SetUpdateFrequency(int freq)
        {
            this.UpdateFrequency = freq;
            this.UpdateTableTimer.Stop();
            this.UpdateTableTimer.Interval = (int)(1000 / this.UpdateFrequency);
            if(this.UdpListener.Running)
                this.UpdateTableTimer.Start();
        }

        public int GetUpdateFrequency()
        {
            return this.UpdateFrequency;
        }

        public DataGridView getLeaderboardGridView()
        {
            return this.dataGridView_leaderboard;
        }

        public void UpdateData()
        {
            if (this.LapPacket == null || this.ParticipantPacket == null || this.SessionPacket == null || this.CarStatusPacket == null || this.CarDamagePacket == null) return;

            if (this.CurrentSessionType != this.SessionPacket.SessionTypeMode)
            {
                this.CurrentSessionType = this.SessionPacket.SessionTypeMode;
                this.LeaderboardManager.InitSessionInfo(NumberOfDrivers, this.SessionPacket.TotalLapsInRace, this.SessionPacket.TrackLengthMeters);
            }

            LeaderboardManager.UpdateData(this.ParticipantPacket, this.LapPacket, this.CarStatusPacket, this.SessionPacket, this.CarDamagePacket); // Update the LeaderboardManager                        
        }

        private void UpdateTable(object sender, EventArgs e)
        {
            if (this.LeaderboardManager.IsReady == false) return;

            this.UpdateSessionInfo();
            
            Leaderboard leaderboard = LeaderboardManager.getLeaderboard(); // Get Current Leaderboard
            this.LatestLeaderboard = leaderboard;

            if (leaderboard.TheoredicalBestLap != 0)
            {
                label_theoredicalBestLap.Invoke((MethodInvoker)delegate ()
                {
                    label_theoredicalBestLap.Text = Utility.formatTimeToMinutes(leaderboard.TheoredicalBestLap); // Theoredical best lap time
                });
            }

            for (byte i = 0; i < NumberOfDrivers; i++) // iterate through positions
            {
                LeaderboardDriver driver = leaderboard.getDriver(i); // Driver in position i
                bool driverIsPlayerOrSpactator = this.SessionPacket.IsSpectating == false && driver.Index == this.SessionPacket.PlayerCarIndex ||
                            this.SessionPacket.IsSpectating == true && driver.Index == this.SessionPacket.CarIndexBeingSpectated;

                dataGridView_leaderboard.Invoke((MethodInvoker)delegate () // Start table editing
                {
                    if (driver == null) // If driver i doesn't exist, clear table row
                    {
                        dataGridView_leaderboard.Rows[i].Cells[0].Style.BackColor = (i % 2 == 0) ? Color.Black : GlobalColorGray; // Color team color field
                        for (byte k = 0; k < dataGridView_leaderboard.Columns.Count; k++) // Remove text from every cell
                            dataGridView_leaderboard.Rows[i].Cells[k].Value = "";
                    }
                    else
                    {
                        dataGridView_leaderboard.Rows[i].SetValues(driver.getDataAsArray()); // Set leaderboard values to table                                                                                        
                        if (driverIsPlayerOrSpactator) // Safe position and driver information of player/spectated player
                        {
                            this.UpdateGapLabels(leaderboard, driver, i); // Update labels for gaps
                            this.LapTimeGraph.UpdateData(this.LeaderboardManager.GetSessionHistories(), driver);
                            this.UpdateDamageLabels(driver);
                        }


                        bool highlight = HighLightOtherPlayers || (HighlightOwnPlayer && driverIsPlayerOrSpactator);
                        if (!PerformanceMode) this.ColorDriverRow(driver, this.ParticipantPacket, i, highlight); // Color the information of the driver
                    }
                }); // end table editing
            }
        }
        public void UpdateHistoryPacket(SessionHistoryPacket sessionHistoryPacket)
        {
            this.LeaderboardManager.UpdateHistoryPacket(sessionHistoryPacket);
        }

        /// <summary>
        /// Color certain columns of a driver row. 
        /// 
        /// Example: If the driver has the best lap, mark it purple
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="participantPacket"></param>
        /// <param name="index"></param>
        private void ColorDriverRow(LeaderboardDriver driver, ParticipantPacket participantPacket, int index, bool highlightPlayer = false)
        {
            Color BlackOrGray = (index % 2 == 0) ? Color.Black : GlobalColorGray;
            DataGridViewRow driversRow = dataGridView_leaderboard.Rows[index];

            driversRow.Cells[0].Style.BackColor = driver.TeamColor; // Color the team color cell

            if (participantPacket.FieldParticipantData[driver.Index].IsAiControlled == false && highlightPlayer) // Driver is human player and should be highlighted
            {
                Color driverBackColor = Color.FromArgb((int)(driver.TeamColor.R * 0.5f), (int)(driver.TeamColor.G * 0.5f), (int)(driver.TeamColor.B * 0.5f)); // 50% of team color

                driversRow.Cells[2].Style.BackColor = driverBackColor; // Name
                driversRow.Cells[3].Style.BackColor = driverBackColor; // Lap number
                driversRow.Cells[19].Style.BackColor = driverBackColor; // tyre
            }
            else
            {
                // Color hightlight fields to default

                driversRow.Cells[2].Style.BackColor = BlackOrGray; // Default black/gray
                driversRow.Cells[3].Style.BackColor = BlackOrGray; // Default black/gray
                driversRow.Cells[19].Style.BackColor = BlackOrGray; // Default black/gray
            }

            driversRow.Cells[4].Style.BackColor = driver.BestLapTimeColor; // Color best lap cell

            driversRow.Cells[7].Style.ForeColor = driver.LastLapTimeColor; // Color last lap cell

            String positionDifference = driversRow.Cells[20].Value.ToString(); // Position difference from start grid
            if (positionDifference.StartsWith("+"))
            {
                driversRow.Cells[20].Style.ForeColor = Color.LimeGreen;
            }
            else if (positionDifference.StartsWith("-"))
            {
                driversRow.Cells[20].Style.ForeColor = Color.Red;
            }
            else
            {
                driversRow.Cells[20].Style.ForeColor = Color.White;
            }

            driversRow.Cells[13].Style.BackColor = driver.Sector1BestColor; // Color best sector 1

            driversRow.Cells[14].Style.ForeColor = driver.Sector1LastColor; // Color last sector 1

            driversRow.Cells[15].Style.BackColor = driver.Sector2BestColor; // Color best sector 2

            driversRow.Cells[16].Style.ForeColor = driver.Sector2LastColor; // Color last sector 2

            driversRow.Cells[17].Style.BackColor = driver.Sector3BestColor; // Color best sector 3

            driversRow.Cells[18].Style.ForeColor = driver.Sector3LastColor; // Color last sector 3

            // Driver overtake marking
            if (driver.HasOvertaken == true)
            {
                UnmarkPositionChanges(); // Undo current marks
                driversRow.Cells[1].Style.BackColor = Color.Green; // Set driver color to green
                for (int k = index + 1; k < driver.OvertakenPosition; k++) // Color of overtaken drivers to red
                {
                    dataGridView_leaderboard.Rows[k].Cells[1].Style.BackColor = Color.DarkRed;
                }
                UnmarkPositionTimer.Start(); // Start timer to unmark
            }

            // Color by pit status
            switch (driversRow.Cells[12].Value)
            {
                case "InGarage":
                    driversRow.Cells[12].Style.ForeColor = Color.Red;
                    break;
                case "OutLap":
                    driversRow.Cells[12].Style.ForeColor = Color.Orange;
                    break;
                case "FlyingLap":
                case "OnTrack":
                    driversRow.Cells[12].Style.ForeColor = Color.LimeGreen;
                    break;
                case "InLap":
                    driversRow.Cells[12].Style.ForeColor = Color.Orange;
                    break;
            }

            // Highlight 'In Pit' with red
            if (driversRow.Cells[11].Value.ToString() == "In Pit")
            {
                driversRow.Cells[11].Style.ForeColor = Color.Red;
            }
            else
            {
                driversRow.Cells[11].Style.ForeColor = Color.White;
            }

            // Highlight if DRS is allowed for the driver
            /*if(driver.DrsAllowed)
            {
                driversRow.Cells[9].Style.BackColor = Color.LimeGreen;
            } else
            {
                driversRow.Cells[9].Style.BackColor = BlackOrGray;
            }*/
        }


        /// <summary>
        /// Updates the labels for gaps of driver ahead, behind and the leader.
        /// </summary>
        /// <param name="leaderboard"></param>
        /// <param name="driver"></param>
        /// <param name="driverPosition"></param>
        private void UpdateGapLabels(Leaderboard leaderboard, LeaderboardDriver driver, int driverPosition)
        {
            // Set text for gap leader
            label_gapLeader.Invoke((MethodInvoker)delegate ()
            {
                label_gapLeader.Text = driver.getGapLeader();
            });

            // Set text for gap interval
            label_gapFront.Invoke((MethodInvoker)delegate ()
            {
                label_gapFront.Text = driver.getGapInterval();
            });


            // Set text for gap back
            string label_gapBack_text = "";
            if (driverPosition < NumberOfDrivers - 1) // If not last driver
            {
                LeaderboardDriver driverBehind = leaderboard.getDriver(driverPosition + 1);
                if (driver != null && driverBehind != null)
                    label_gapBack_text = driverBehind.getGapInterval();
            }
            label_gapBack.Invoke((MethodInvoker)delegate ()
            {
                label_gapBack.Text = label_gapBack_text;
            });

        }

        private void UpdateDamageLabels(LeaderboardDriver driver)
        {
            this.label_frontWingDamageLeft.Invoke((MethodInvoker)delegate ()
            {
                this.label_frontWingDamageLeft.Text = driver.frontWingDamageLeft + "%";
                if(driver.frontWingDamageLeft == 0)
                {
                    this.label_frontWingDamageLeft.ForeColor = Color.LimeGreen;
                } else
                {
                    this.label_frontWingDamageLeft.ForeColor = Color.Red;
                }
            });

            this.label_frontWingDamageRight.Invoke((MethodInvoker)delegate ()
            {
                this.label_frontWingDamageRight.Text = driver.frontWingDamageRight + "%";
                if (driver.frontWingDamageRight == 0)
                {
                    this.label_frontWingDamageRight.ForeColor = Color.LimeGreen;
                }
                else
                {
                    this.label_frontWingDamageRight.ForeColor = Color.Red;
                }
            });
        }

        /// <summary>
        /// Unmark the position change coloring and stop the timer
        /// </summary>
        private void UnmarkPositionChanges()
        {
            UnmarkPositionTimer.Stop(); // Stop the timer
            for (int i = 0; i < NumberOfDrivers; i++)
            {
                dataGridView_leaderboard.Rows[i].Cells[1].Style.BackColor = (i % 2 == 0) ? Color.Black : GlobalColorGray;
            }
        }

        /// <summary>
        /// Handle new session packet. Changes components such as marshalzones, session timer and track label
        /// </summary>
        /// <param name="sessionPacket"></param>
        private void UpdateSessionInfo()
        {           
            label_sessionTimeLeft.Invoke((MethodInvoker)delegate ()
            {
                label_sessionTimeLeft.Text = Utility.formatTimeToMinutes(this.SessionPacket.SessionTimeLeft, false, true); // Set session time left text
            });

            if (this.SessionPacket.SessionTimeLeft < 300) // If less than 5 minutes
            {
                label_sessionTimeLeft.ForeColor = Color.Red; // Text color red
                TimeLeftBlinkTimer.Start(); // Start blinking
            }
            else // More than 5 minutes
            {
                label_sessionTimeLeft.ForeColor = Color.White; // Text color white
                TimeLeftBlinkTimer.Stop(); // Stop blinking 
                label_sessionTimeLeft.BackColor = Color.Transparent; // Background transparent
            }

            for (int i = 0; i < 21; i++) // For every marshalzone set color
            {
                if (i + 1 > this.SessionPacket.NumberOfMarshallZones)
                {
                    MarshalZoneLabels[i].BackColor = Color.Transparent;
                }
                else
                {
                    SessionPacket.MarshallZone mz = this.SessionPacket.MarshallZones[i];
                    switch (mz.ZoneFlag)
                    {
                        case FiaFlag.None:
                        case FiaFlag.Green:
                            MarshalZoneLabels[i].BackColor = Color.LimeGreen;
                            break;
                        case FiaFlag.Blue:
                            MarshalZoneLabels[i].BackColor = Color.Blue;
                            break;
                        case FiaFlag.Yellow:
                            MarshalZoneLabels[i].BackColor = Color.Yellow;
                            break;
                        case FiaFlag.Red:
                            MarshalZoneLabels[i].BackColor = Color.Red;
                            break;
                        default:
                            MarshalZoneLabels[i].BackColor = Color.Transparent;
                            break;
                    }
                }
            }
            if (this.SessionPacket.CurrentSafetyCarStatus != SessionPacket.SafetyCarStatus.None) // If there is (virtual) safety car
            {
                label_safetyCar.Invoke((MethodInvoker)delegate ()
                {
                    label_safetyCar.Text = this.SessionPacket.CurrentSafetyCarStatus.ToString(); // Safety car status
                });
                SafetyCarBlinkTimer.Start();
            }
            else // If there is no safety car
            {
                SafetyCarBlinkTimer.Stop();
                label_safetyCar.Invoke((MethodInvoker)delegate ()
                {
                    label_safetyCar.Text = "SC";
                    label_safetyCar.BackColor = Color.Transparent;
                    label_safetyCar.ForeColor = Color.Black;
                });
            }

            label_session.Invoke((MethodInvoker)delegate ()
            {
                label_session.Text = this.SessionPacket.SessionTypeMode.ToString(); // Set session type text
            });

            label_track.Invoke((MethodInvoker)delegate ()
            {
                label_track.Text = this.SessionPacket.SessionTrack.ToString(); // Set track name
            });

            label_airTemp.Invoke((MethodInvoker)delegate ()
            {
                label_airTemp.Text = this.SessionPacket.AirTemperatureCelsius + " °C"; // Set air temperature
            });

            label_trackTemp.Invoke((MethodInvoker)delegate ()
            {
                label_trackTemp.Text = this.SessionPacket.TrackTemperatureCelsius + " °C"; // Set track temperature
            });

            label_pitstopIdealLap.Invoke((MethodInvoker)delegate ()
            {
                byte idealLap = this.SessionPacket.PitStopWindowIdealLap;
                if (idealLap > 0)
                    label_pitstopIdealLap.Text = idealLap.ToString(); // Pit stop ideal lap (current strategy)
                else label_pitstopIdealLap.Text = "-";
            });

            label_pitstopLatestLap.Invoke((MethodInvoker)delegate ()
            {
                byte latestLap = this.SessionPacket.PitStopWindowLatestLap;
                if (latestLap > 0)
                    label_pitstopLatestLap.Text = latestLap.ToString(); // Pit stop latest lap (current strategy)
                else label_pitstopLatestLap.Text = "-";
            });

            label_pitstopPositionAfter.Invoke((MethodInvoker)delegate ()
            {
                label_pitstopPositionAfter.Text = this.SessionPacket.PitStopRejoinPosition.ToString(); // Estimated position after pit stop
            });


            for (int i = 0; i < 5; i++) // For every forecast label
            {
                Label rainPercentageLabel = RainPercentageLabels[i];
                Label weatherForecastLabel = WeatherForecastLabels[i];
                string rainPercentage;
                string weatherForecast;
                if (this.SessionPacket.SessionTypeMode == this.SessionPacket.WeatherForecastSamples[i].SessionTypeMode)
                {
                    rainPercentage = this.SessionPacket.WeatherForecastSamples[i].RainPercentage + "%";
                    weatherForecast = this.SessionPacket.WeatherForecastSamples[i].ForecastedWeatherCondition.ToString();
                }
                else
                {
                    rainPercentage = "-";
                    weatherForecast = "-";
                }

                rainPercentageLabel.Invoke((MethodInvoker)delegate ()
                {
                    rainPercentageLabel.Text = rainPercentage;
                });

                weatherForecastLabel.Invoke((MethodInvoker)delegate ()
                {
                    weatherForecastLabel.Text = weatherForecast;
                });
            }
        }

        private void CheckPitstopDelta()
        {
            if (this.SessionPacket != null)
            {
                DriverCircle.UseAutomaticPitstopDelta(); // Tell driver circle to use automatic mode
                float delta = DriverCircle.TrackPitstopDelta[(int)this.SessionPacket.SessionTrack]; // Recommended delta
                numeric_pitstopDelta.Value = (decimal)delta; // Set input component to recommended delta
            }
        }

        public void ClearTableColoring()
        {
            for (int i = 0; i < dataGridView_leaderboard.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView_leaderboard.Rows[i].Cells.Count; k++)
                {
                    dataGridView_leaderboard.Rows[i].Cells[k].Style.ForeColor = Color.White;
                    dataGridView_leaderboard.Rows[i].Cells[k].Style.BackColor = (i % 2 == 0) ? Color.Black : GlobalColorGray;
                }
            }
        }

        public void SetLiveTimingEnabled(bool enabled)
        {
            LeaderboardManager.LiveTiming = enabled;
        }

        public bool getLiveTimingEnabled()
        {
            return LeaderboardManager.LiveTiming;
        }

        public void SetWeatherVisibility(bool visible)
        {
            this.ShowWeather = visible;
            if (this.ShowWeather == false && this.gb_weatherForecast.Visible)
            {
                this.gb_weatherForecast.Visible = false;
                this.gb_lapTimes.Location = new Point(this.gb_weatherForecast.Location.X, this.gb_lapTimes.Location.Y);
                //this.gb_lapTimes.Width = gb_lapTimes.Width + gb_weatherForecast.Width + 5;
            }
            else if (this.ShowWeather && this.gb_weatherForecast.Visible == false)
            {
                this.gb_weatherForecast.Visible = true;
                this.gb_lapTimes.Location = new Point(this.gb_weatherForecast.Location.X + gb_weatherForecast.Width + 5, this.gb_lapTimes.Location.Y);
                //this.gb_lapTimes.Width = 265;
            }
            this.ResizeUi();
        }

        private void ResizeUi()
        {
            if (pb_driverCircle.Height > 10) // Prevent glitch when minimizing
            {
                pb_driverCircle.Width = pb_driverCircle.Height; // Make driver circle base form a square again
                pb_driverCircle.Location = new Point(dataGridView_leaderboard.Location.X + dataGridView_leaderboard.Width - pb_driverCircle.Width, pb_driverCircle.Location.Y); // Relocate driver circle
                if (pb_driverCircle.Height > 0)
                    this.DriverCircle.Resize();

                label_text_pitstopDelta.Location = new Point(pb_driverCircle.Location.X - 208, label_text_pitstopDelta.Location.Y); // Move label next to driver circle
                numeric_pitstopDelta.Location = new Point(pb_driverCircle.Location.X - 90, numeric_pitstopDelta.Location.Y); // Move pitstopDeltaInput next to driver circle
                button_pitstopDeltaAuto.Location = new Point(pb_driverCircle.Location.X - 90, button_pitstopDeltaAuto.Location.Y); // Move pitstopDeltaAutoCheckbox next to driver circle
            }
            if (gb_lapTimes.Width > 10)
            {
                gb_lapTimes.Width = pb_driverCircle.Location.X - gb_lapTimes.Location.X - 5;
                gb_lapTimes.Height = this.Height - gb_lapTimes.Location.Y - 51;
            }
        }

        /*
         * UI Events following
         * 
         * */

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView_leaderboard.ClearSelection(); // Clear table selection on startup
        }

        public void FormSettingsClosed()
        {
            this.SettingsOpen = false;
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            ResizeUi();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UdpListener.Stop(); // Stop UDP listener (other Thread, importent to close)
        }
        private void button_clearTableSelection_Click(object sender, EventArgs e)
        {
            dataGridView_leaderboard.ClearSelection(); // Clear selection of the leaderboard table
        }

        /// <summary>
        /// Timer event after the timer is over. Calls unmarking method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnmarkPositionChanges(object sender, ElapsedEventArgs e)
        {
            UnmarkPositionChanges();
        }

        /// <summary>
        /// Timer event after blink timer is over for 'session time left'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeLeftBlink(object sender, ElapsedEventArgs e)
        {
            if (label_sessionTimeLeft.BackColor == Color.Transparent)
            {
                label_sessionTimeLeft.BackColor = Color.White;
            }
            else
            {
                label_sessionTimeLeft.BackColor = Color.Transparent;
            }
        }

        private void OnSafetyCarBlink(object sender, ElapsedEventArgs e)
        {
            if (label_safetyCar.BackColor != Color.Yellow)
            {
                label_safetyCar.BackColor = Color.Yellow;
                label_safetyCar.ForeColor = Color.Black;
            }
            else
            {
                label_safetyCar.BackColor = Color.Transparent;
                label_safetyCar.ForeColor = Color.White;
            }
        }

        private void numeric_pitstopDelta_ValueChanged(object sender, EventArgs e)
        {
            DriverCircle.SetCustomPitstopDelta((float)numeric_pitstopDelta.Value); // Tell driver circle to use custom delta
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            if (!this.SettingsOpen)
            {
                this.FormSettings = new FormSettings(this);
                this.FormSettings.Show();
            }
            this.SettingsOpen = true;
        }

        private void button_pitstopDeltaAuto_Click(object sender, EventArgs e)
        {
            this.CheckPitstopDelta();
        }
    }
}
