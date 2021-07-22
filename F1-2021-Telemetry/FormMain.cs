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
        static readonly Color GlobalColorGray = Color.FromArgb(50, 50, 50);
        static readonly int NumberOfDrivers = 20;

        private UdpListener UdpListener; // Receiving data from the game

        private LeaderboardManager LeaderboardManager; // leadboard table

        private DriverCircle DriverCircle; // driver circle
        private Bitmap CircleBitmap; // Bitmap for driver circle

        private SessionPacket SessionPacket;

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

        public FormMain()
        {
            InitializeComponent();

            LeaderboardManager = new LeaderboardManager();
            DriverCircle = new DriverCircle();
            UdpListener = new UdpListener(this); // Create UDP listener

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

            CircleBitmap = new Bitmap(pb_driverCircle.Width, pb_driverCircle.Height); // Create bitmap for driver circle

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
        }

        public DataGridView getLeaderboardGridView()
        {
            return this.dataGridView_leaderboard;
        }

        /// <summary>
        /// Update everything with the given data packets
        /// </summary>
        /// <param name="participantPacket">ParticipantPacket</param>
        /// <param name="lapPacket">LapPacket</param>
        /// <param name="carStatusPacket">CarStatusPacket</param>
        /// <param name="sessionPacket">SessionPacket</param>
        /// <param name="sessionHistoryPacket">SessionPacket</param>
        public void UpdateData(ParticipantPacket participantPacket, LapPacket lapPacket, CarStatusPacket carStatusPacket, SessionPacket sessionPacket)
        {
            this.UpdateSession(sessionPacket); // Update session related information                

            LeaderboardManager.UpdateData(participantPacket, lapPacket, carStatusPacket, sessionPacket); // Update the LeaderboardManager
            Leaderboard leaderboard = LeaderboardManager.getLeaderboard(); // Get Current Leaderboard

            if (leaderboard.TheoredicalBestLap != 0)
            {
                label_theoredicalBestLap.Invoke((MethodInvoker)delegate ()
                {
                    label_theoredicalBestLap.Text = Utility.formatTimeToMinutes(leaderboard.TheoredicalBestLap); // Theoredical best lap time
                });
            }

            this.UpdateCircle(leaderboard, lapPacket, sessionPacket); // Update driver circle


            int playerPosition = 0; // Position of player
            LeaderboardDriver player = null; // Data of Player

            for (byte i = 0; i < NumberOfDrivers; i++) // iterate through positions
            {
                LeaderboardDriver driver = leaderboard.getDriver(i); // Driver in position i

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
                        if (sessionPacket.IsSpectating == false && driver.Index == sessionPacket.PlayerCarIndex ||
                            sessionPacket.IsSpectating == true && driver.Index == sessionPacket.CarIndexBeingSpectated) // Safe position and driver information of player/spectated player
                        {
                            playerPosition = i;
                            player = driver;
                        }

                        dataGridView_leaderboard.Rows[i].SetValues(driver.getDataAsArray()); // Set leaderboard values to table                                                                                        

                        bool highlight = HighLightOtherPlayers || (HighlightOwnPlayer && (driver.Index == participantPacket.PlayerCarIndex || (sessionPacket.IsSpectating && driver.Index == sessionPacket.CarIndexBeingSpectated)));
                        if (!PerformanceMode) this.ColorDriverRow(driver, participantPacket, i, highlight); // Color the information of the driver
                        if (driver.Index == lapPacket.PlayerCarIndex) this.PlotLapTimeGraph(this.LeaderboardManager.GetSessionHistories(), leaderboard, driver);
                    }
                }); // end table editing
            }

            if (player != null)
            {
                this.UpdateGapLabels(leaderboard, player, playerPosition); // Update labels for gaps
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

            if (participantPacket.FieldParticipantData[driver.Index].IsAiControlled == false && highlightPlayer) // Driver is human player
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
        /// Updates the driver circle
        /// </summary>
        /// <param name="leaderboard"></param>
        /// <param name="lapPacket"></param>
        /// <param name="sessionPacket"></param>
        private void UpdateCircle(Leaderboard leaderboard, LapPacket lapPacket, SessionPacket sessionPacket)
        {
            pb_driverCircle.Invoke((MethodInvoker)delegate () // Draw circle with all drivers
            {
                DriverCircle.ClearCircle(pb_driverCircle, CircleBitmap, GlobalColorGray); // Clear the driver circle
                for (byte i = 0; i < NumberOfDrivers; i++)
                {
                    LeaderboardDriver driver = leaderboard.getDriver(NumberOfDrivers - 1 - i); // Driver in position i
                    if (driver != null)
                    {
                        DriverCircle.DrawDrivers(driver, lapPacket, sessionPacket, pb_driverCircle, CircleBitmap); // Tell circle to draw driver
                    }
                }
            });
        }

        /// <summary>
        /// Plot the last 10 lap times of the driver (and the driver in front/back)
        /// </summary>
        /// <param name="sessionHistoryPackets"></param>
        /// <param name="leaderboard"></param>
        /// <param name="driver"></param>
        private void PlotLapTimeGraph(SessionHistoryPacket[] sessionHistoryPackets, Leaderboard leaderboard, LeaderboardDriver driver)
        {
            if (sessionHistoryPackets[driver.Index] == null)
            {
                return; // If no history data for self
            }

            List<double> lapTimesSelf = new List<double>();
            List<double> lapTimesFront = new List<double>();
            List<double> lapTimesBehind = new List<double>();


            LeaderboardDriver driverFront = null;
            if (driver.CarPosition > 1)
                driverFront = leaderboard.DriverData[driver.CarPosition - 2];

            LeaderboardDriver driverBehind = null;
            if (driver.CarPosition < 20)
                driverBehind = leaderboard.DriverData[driver.CarPosition];

            bool driverFrontValid = driverFront != null && sessionHistoryPackets[driverFront.Index] != null; // whether a driver in front exists (and its data)
            bool driverBehindValid = driverBehind != null && sessionHistoryPackets[driverBehind.Index] != null; // whether a driver behind exists (and its data)

            int lapStartIndex = driver.CurrentLapNumber > 10 ? driver.CurrentLapNumber - 10 - 1 : 0; // Current lap number - 10

            for(int i = 0; i < lapStartIndex; i++) // Offset of lines in graph so the laps are visibile and in the right spot
            {
                lapTimesSelf.Add(0);
                lapTimesFront.Add(0); 
                lapTimesBehind.Add(0);
            }

            double min = 1000; // min lap time
            double max = 0; // max lap time

            for (int i = lapStartIndex; i < lapStartIndex + 10; i++) // for each lap
            {
                // Self
                double lapTime = sessionHistoryPackets[driver.Index].LapsHistoryData[i].LapTimeInMs / 1000.0; // Lap time in seconds
                if(lapTime != 0)
                    lapTimesSelf.Add(lapTime); // Add time to list

                if (lapTime > max)
                {
                    max = lapTime;
                }
                if (lapTime < min && lapTime > 0)
                {
                    min = lapTime;
                }


                // Driver in front
                if (driverFrontValid)
                {
                    lapTime = sessionHistoryPackets[driverFront.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesFront.Add(lapTime);
                    if (lapTime > max)
                    {
                        max = lapTime;
                    }
                    if (lapTime < min && lapTime > 0)
                    {
                        min = lapTime;
                    }
                }

                // Driver behind
                if (driverBehindValid)
                {
                    lapTime = sessionHistoryPackets[driverBehind.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesBehind.Add(lapTime);
                    if (lapTime > max)
                    {
                        max = lapTime;
                    }
                    if (lapTime < min && lapTime > 0)
                    {
                        min = lapTime;
                    }
                }
            }

            // Create series for self
            Series selfSeries = new Series("Self");
            selfSeries.Points.DataBindY(lapTimesSelf.ToArray()); // Convert list to data points
            selfSeries.ChartType = SeriesChartType.FastLine;
            selfSeries.Color = driver.TeamColor;
            selfSeries.BorderWidth = 6; // Own line thicker than the others

            Series driverFrontSeries = new Series("Front");
            if (driverFrontValid)
            {
                driverFrontSeries.Points.DataBindY(lapTimesFront.ToArray());
                driverFrontSeries.ChartType = SeriesChartType.FastLine;
                driverFrontSeries.Color = driverFront.TeamColor;
                driverFrontSeries.BorderWidth = 3;
            }

            Series driverBehindSeries = new Series("Behind");
            if (driverBehindValid)
            {
                driverBehindSeries.Points.DataBindY(lapTimesBehind.ToArray());
                driverBehindSeries.ChartType = SeriesChartType.FastLine;
                driverBehindSeries.Color = driverBehind.TeamColor;
                driverBehindSeries.BorderWidth = 3;
            }

            // Add each series to the chart
            chart1.Series.Clear();
            chart1.Series.Add(selfSeries);
            chart1.Series.Add(driverFrontSeries);
            chart1.Series.Add(driverBehindSeries);

            min = Math.Floor(min);
            max = Math.Ceiling(max);

            // Additional styling
            chart1.ResetAutoValues();
            chart1.Titles.Clear();
            chart1.ChartAreas[0].AxisY.Interval = Math.Round((max - min) / 5.0,0);
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Minimum = min - 1;
            chart1.ChartAreas[0].AxisY.Maximum = max + 1;
            chart1.ChartAreas[0].AxisX.Minimum = lapStartIndex + 1;
            chart1.ChartAreas[0].AxisX.Maximum = lapStartIndex + 10;
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
        private void UpdateSession(SessionPacket sessionPacket)
        {
            if (this.SessionPacket == null ||
                sessionPacket.SessionTime < this.SessionPacket.SessionTime && sessionPacket.SessionTypeMode != this.SessionPacket.SessionTypeMode) // New session 
            {
                Console.WriteLine("New Session");
                this.LeaderboardManager.InitSessionInfo(NumberOfDrivers, sessionPacket.TotalLapsInRace, sessionPacket.TrackLengthMeters); // Init leaderboard manager
            }

            this.SessionPacket = sessionPacket;

            label_sessionTimeLeft.Invoke((MethodInvoker)delegate ()
            {
                label_sessionTimeLeft.Text = Utility.formatTimeToMinutes(sessionPacket.SessionTimeLeft, false, true); // Set session time left text
            });

            if (sessionPacket.SessionTimeLeft < 300) // If less than 5 minutes
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
                if (i + 1 > sessionPacket.NumberOfMarshallZones)
                {
                    MarshalZoneLabels[i].BackColor = Color.Transparent;
                }
                else
                {
                    SessionPacket.MarshallZone mz = sessionPacket.MarshallZones[i];
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

            label_session.Invoke((MethodInvoker)delegate ()
            {
                label_session.Text = sessionPacket.SessionTypeMode.ToString();
            });

            label_track.Invoke((MethodInvoker)delegate ()
            {
                label_track.Text = sessionPacket.SessionTrack.ToString(); // Set track name
            });

            label_airTemp.Invoke((MethodInvoker)delegate ()
            {
                label_airTemp.Text = sessionPacket.AirTemperatureCelsius + " °C"; // Set air temperature
            });

            label_trackTemp.Invoke((MethodInvoker)delegate ()
            {
                label_trackTemp.Text = sessionPacket.TrackTemperatureCelsius + " °C"; // Set track temperature
            });

            label_pitstopIdealLap.Invoke((MethodInvoker)delegate ()
            {
                byte idealLap = sessionPacket.PitStopWindowIdealLap;
                if (idealLap > 0)
                    label_pitstopIdealLap.Text = idealLap.ToString(); // Pit stop ideal lap (current strategy)
                else label_pitstopIdealLap.Text = "-";
            });

            label_pitstopLatestLap.Invoke((MethodInvoker)delegate ()
            {
                byte latestLap = sessionPacket.PitStopWindowLatestLap;
                if (latestLap > 0)
                    label_pitstopLatestLap.Text = latestLap.ToString(); // Pit stop latest lap (current strategy)
                else label_pitstopLatestLap.Text = "-";
            });

            label_pitstopPositionAfter.Invoke((MethodInvoker)delegate ()
            {
                label_pitstopPositionAfter.Text = sessionPacket.PitStopRejoinPosition.ToString(); // Estimated position after pit stop
            });

            if (sessionPacket.CurrentSafetyCarStatus != SessionPacket.SafetyCarStatus.None)
            {
                label_safetyCar.Invoke((MethodInvoker)delegate ()
                {
                    label_safetyCar.Text = sessionPacket.CurrentSafetyCarStatus.ToString();
                });
                SafetyCarBlinkTimer.Start();
            }
            else
            {
                SafetyCarBlinkTimer.Stop();
                label_safetyCar.Invoke((MethodInvoker)delegate ()
                {
                    label_safetyCar.Text = "";
                    label_safetyCar.BackColor = Color.Transparent;
                });
            }

            for (int i = 0; i < 5; i++)
            {
                Label rainPercentageLabel = RainPercentageLabels[i];
                Label weatherForecastLabel = WeatherForecastLabels[i];
                string rainPercentage;
                string weatherForecast;
                if (sessionPacket.SessionTypeMode == sessionPacket.WeatherForecastSamples[i].SessionTypeMode)
                {
                    rainPercentage = sessionPacket.WeatherForecastSamples[i].RainPercentage + "%";
                    weatherForecast = sessionPacket.WeatherForecastSamples[i].ForecastedWeatherCondition.ToString();
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
            if (pb_driverCircle.Height > 10) // Prevent glitch when minimizing
            {
                pb_driverCircle.Width = pb_driverCircle.Height; // Make driver circle base form a square again
                pb_driverCircle.Location = new Point(dataGridView_leaderboard.Location.X + dataGridView_leaderboard.Width - pb_driverCircle.Width, pb_driverCircle.Location.Y); // Relocate driver circle
                if (pb_driverCircle.Height > 0)
                    CircleBitmap = new Bitmap(pb_driverCircle.Width, pb_driverCircle.Height); // Adjust bitmap size within picturebox

                label_text_pitstopDelta.Location = new Point(pb_driverCircle.Location.X - 208, label_text_pitstopDelta.Location.Y); // Move label next to driver circle
                numeric_pitstopDelta.Location = new Point(pb_driverCircle.Location.X - 90, numeric_pitstopDelta.Location.Y); // Move pitstopDeltaInput next to driver circle
                button_pitstopDeltaAuto.Location = new Point(pb_driverCircle.Location.X - 90, button_pitstopDeltaAuto.Location.Y); // Move pitstopDeltaAutoCheckbox next to driver circle
            }
            if(gp_lapTimes.Width > 10)
            {
                gp_lapTimes.Width = pb_driverCircle.Location.X - gp_lapTimes.Location.X - 5;
                gp_lapTimes.Height = this.Height - gp_lapTimes.Location.Y - 51;
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UdpListener.Stop(); // Stop UDP listener (other Thread, importent to close)
        }

        /// <summary>
        /// Button start/stop has been pressed: start/stop the UDP listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_start_Click(object sender, EventArgs e)
        {
            if (UdpListener.Running)
            {
                UdpListener.Stop();
                button_start.Text = "Start";
                button_start.BackColor = Color.LimeGreen;
            }
            else
            {
                UdpListener.Start();
                button_start.Text = "Stop";
                button_start.BackColor = Color.LightGray;
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            LeaderboardManager.Clear(); // Reset leaderboard
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
