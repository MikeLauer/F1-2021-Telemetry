using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace F1_2021_Telemetry
{
    public partial class FormSettings : Form
    {
        FormMain MainForm;

        public FormSettings(FormMain mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            this.Location = this.MainForm.Location;

            cb_liveTimings.Checked = this.MainForm.getLiveTimingEnabled();

            cb_showWeather.Checked = this.MainForm.ShowWeather;

            DataGridView leaderboardTable = MainForm.getLeaderboardGridView();
            cb_show_lapNumber.Checked = leaderboardTable.Columns[3].Visible;
            cb_show_bestLap.Checked = leaderboardTable.Columns[4].Visible;
            cb_show_deltaBestLeader.Checked = leaderboardTable.Columns[5].Visible;
            cb_show_deltaBestInterval.Checked = leaderboardTable.Columns[6].Visible;
            cb_show_lastLap.Checked = leaderboardTable.Columns[7].Visible;
            cb_show_deltaLastInterval.Checked = leaderboardTable.Columns[8].Visible;
            cb_show_gapInterval.Checked = leaderboardTable.Columns[9].Visible;
            cb_show_gapLeader.Checked = leaderboardTable.Columns[10].Visible;
            cb_show_pitStatus.Checked = leaderboardTable.Columns[11].Visible;
            cb_show_status.Checked = leaderboardTable.Columns[12].Visible;
            cb_show_bestSector.Checked = leaderboardTable.Columns[13].Visible; // 15 17 respectively
            cb_show_lastSector.Checked = leaderboardTable.Columns[14].Visible; // 16 18 respectively
            cb_show_tyre.Checked = leaderboardTable.Columns[19].Visible;
            cb_show_positionDifference.Checked = leaderboardTable.Columns[20].Visible;
            cb_show_timePenalties.Checked = leaderboardTable.Columns[21].Visible;

            cb_customDriverNames.Checked = this.MainForm.CustomDriverNames;
            this.UpdateButtonLoadCustomDriverNames();

            if (this.MainForm.HighLightOtherPlayers) rb_playerHighlight_everyone.Checked = true;
            else if (this.MainForm.HighlightOwnPlayer) rb_playerHighlight_onlyMe.Checked = true;
            else rb_playerHighlight_off.Checked = true;

            cb_performanceMode.Checked = this.MainForm.PerformanceMode;

            int updateFrequency = this.MainForm.GetUpdateFrequency();
            if(updateFrequency == 1)
            {
                this.rb_updateFrequency_1hz.Checked = true;
            }
            else if (updateFrequency == 10)
            {
                this.rb_updateFrequency_10hz.Checked = true;
            }
            else if (updateFrequency == 30)
            {
                this.rb_updateFrequency_30hz.Checked = true;
            }
            else if (updateFrequency == 60)
            {
                this.rb_updateFrequency_60hz.Checked = true;
            }

            int[] lapTimeGraphValues = this.MainForm.GetLapTimeGraphValues();
            nud_lapTimeGraphNumDriversInFront.Value = lapTimeGraphValues[0];
            nud_lapTimeGraphNumDriversBehind.Value = lapTimeGraphValues[1];
            nud_lapTimeGraphNumberOfLapsToShow.Value = lapTimeGraphValues[2];
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            int updateFrequency = 1; // Default
            if(this.rb_updateFrequency_10hz.Checked)
            {
                updateFrequency = 10;
            }
            else if (this.rb_updateFrequency_30hz.Checked)
            {
                updateFrequency = 30;
            }
            else if (this.rb_updateFrequency_60hz.Checked)
            {
                updateFrequency = 60;
            }
            this.MainForm.SetUpdateFrequency(updateFrequency);
            this.Close();
        }

        private bool LoadCustomDriverNamesFromFile()
        {
            openFileDialog_customDriverNames = new OpenFileDialog();
            openFileDialog_customDriverNames.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog_customDriverNames.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog_customDriverNames.FileName;
                //File.WriteAllText(saveFileDialog_finalResult.FileName, jsonString);
                string fileContent = File.ReadAllText(filePath);
                Utility.CustomDriverNamesMap = JsonSerializer.Deserialize<Dictionary<byte, string>>(fileContent);
                this.MainForm.CustomDriverNames = cb_customDriverNames.Checked;
                this.UpdateButtonLoadCustomDriverNames();
                return true;
            } else
            {
                return false;
            }
        }

        public void FinalClassification()
        {
            if (this.MainForm.FinalClassificationPacket == null)
            {
                MessageBox.Show("No results available");
                return;
            }
            Utility.FinalClassification fc = new Utility.FinalClassification();
            fc.Track = this.MainForm.SessionPacket.SessionTrack.ToString();

            Utility.FinalClassificationDriver[] fcd = new Utility.FinalClassificationDriver[this.MainForm.FinalClassificationPacket.NumCars];
            for (int i = 0; i < this.MainForm.FinalClassificationPacket.NumCars; i++)
            {
                Codemasters.F1_2021.FinalClassificationPacket.FinalClassificationData driverData = this.MainForm.FinalClassificationPacket.FieldDriverData[i];

                Utility.FinalClassificationDriver driver = new Utility.FinalClassificationDriver();
                driver.Name = this.MainForm.ParticipantPacket.FieldParticipantData[i].Name;
                if (this.MainForm.CustomDriverNames)
                {
                    driver.Name = Utility.GetCustomDriverNameFromRaceNumber(this.MainForm.ParticipantPacket.FieldParticipantData[i].CarRaceNumber, this.MainForm.ParticipantPacket.FieldParticipantData[i].Name);
                }
                driver.RaceNumber = this.MainForm.ParticipantPacket.FieldParticipantData[i].CarRaceNumber;
                driver.Position = driverData.Position;
                driver.NumLaps = driverData.NumLaps;
                driver.GridPosition = driverData.GridPosition;
                driver.Points = driverData.Points;
                driver.NumPitStops = driverData.NumPitStops;
                driver.ResultStatus = driverData.ResultStatus.ToString();
                driver.BestLapTimeInMS = driverData.BestLapTimeInMS;
                driver.TotalRaceTimeInSec = driverData.TotalRaceTimeInSec;
                driver.PenaltiesTime = driverData.PenaltiesTime;
                driver.NumPenalties = driverData.NumPenalties;
                driver.NumTyreStins = driverData.NumTyreStins;

                fcd[driver.Position - 1] = driver;
            }

            fc.Drivers = fcd;

            saveFileDialog_raceResult = new SaveFileDialog();
            saveFileDialog_raceResult.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog_raceResult.FileName = this.MainForm.SessionPacket.SessionTrack.ToString() + "_" + DateTime.Now;
            if (saveFileDialog_raceResult.ShowDialog() == DialogResult.OK)
            {
                string jsonString = JsonSerializer.Serialize(fc);
                File.WriteAllText(saveFileDialog_raceResult.FileName, jsonString);
            }

        }

        private void SetLapTimeGraphValues()
        {
            this.MainForm.SetLapTimeGraphValues((int) nud_lapTimeGraphNumDriversInFront.Value, (int) nud_lapTimeGraphNumDriversBehind.Value, (int) nud_lapTimeGraphNumberOfLapsToShow.Value);
        }

        private void cb_show_lapNumber_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[3].Visible = cb_show_lapNumber.Checked;
        }

        private void cb_show_bestLap_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[4].Visible = cb_show_bestLap.Checked;
        }

        private void cb_show_deltaBestLeader_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[5].Visible = cb_show_deltaBestLeader.Checked;
        }

        private void cb_show_deltaBestInterval_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[6].Visible = cb_show_deltaBestInterval.Checked;
        }

        private void cb_show_lastLap_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[7].Visible = cb_show_lastLap.Checked;
        }

        private void cb_show_deltaLastInterval_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[8].Visible = cb_show_deltaLastInterval.Checked;
        }

        private void cb_show_gapInterval_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[9].Visible = cb_show_gapInterval.Checked;
        }

        private void cb_show_gapLeader_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[10].Visible = cb_show_gapLeader.Checked;
        }

        private void cb_show_pitStatus_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[11].Visible = cb_show_pitStatus.Checked;
        }

        private void cb_show_status_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[12].Visible = cb_show_status.Checked;
        }

        private void cb_show_bestSector_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[13].Visible = cb_show_bestSector.Checked;
            this.MainForm.getLeaderboardGridView().Columns[15].Visible = cb_show_bestSector.Checked;
            this.MainForm.getLeaderboardGridView().Columns[17].Visible = cb_show_bestSector.Checked;
        }

        private void cb_show_lastSector_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[14].Visible = cb_show_lastSector.Checked;
            this.MainForm.getLeaderboardGridView().Columns[16].Visible = cb_show_lastSector.Checked;
            this.MainForm.getLeaderboardGridView().Columns[18].Visible = cb_show_lastSector.Checked;
        }

        private void cb_show_tyre_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[19].Visible = cb_show_tyre.Checked;
        }

        private void cb_show_positionDifference_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[20].Visible = cb_show_positionDifference.Checked;
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.MainForm.FormSettingsClosed();
        }

        private void cb_show_timePenalties_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.getLeaderboardGridView().Columns[21].Visible = cb_show_timePenalties.Checked;
        }

        private void cb_performanceMode_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.PerformanceMode = cb_performanceMode.Checked;
            rb_playerHighlight_off.Enabled = !this.cb_performanceMode.Checked;
            rb_playerHighlight_onlyMe.Enabled = !this.cb_performanceMode.Checked;
            rb_playerHighlight_everyone.Enabled = !this.cb_performanceMode.Checked;
            if (this.cb_performanceMode.Checked)
            {
                this.MainForm.ClearTableColoring();
            }
        }

        private void cb_liveTimings_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.SetLiveTimingEnabled(cb_liveTimings.Checked);
        }

        private void rb_playerHighlight_off_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.HighlightOwnPlayer = false;
            this.MainForm.HighLightOtherPlayers = false;
        }

        private void rb_playerHighlight_onlyMe_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.HighlightOwnPlayer = true;
            this.MainForm.HighLightOtherPlayers = false;
        }

        private void rb_playerHighlight_everyone_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.HighlightOwnPlayer = true;
            this.MainForm.HighLightOtherPlayers = true;
        }

        private void cb_showWeather_CheckedChanged(object sender, EventArgs e)
        {
            this.MainForm.SetWeatherVisibility(cb_showWeather.Checked);
        }

        private void cb_customDriverNames_CheckedChanged(object sender, EventArgs e)
        {
            button_loadCustomDriverNames.Enabled = cb_customDriverNames.Checked;

            // Automatically open file dialog if no data is available
            if(Utility.CustomDriverNamesMap == null)
            {
                this.LoadCustomDriverNamesFromFile();
            }

            if (cb_customDriverNames.Checked && Utility.CustomDriverNamesMap != null)
            {
                this.MainForm.CustomDriverNames = true;
            }
            else { 
                this.MainForm.CustomDriverNames = false;
            }
        }

        private void nud_lapTimeGraphNumDriversInFront_ValueChanged(object sender, EventArgs e)
        {
            this.SetLapTimeGraphValues();
        }

        private void nud_lapTimeGraphNumDriversBehind_ValueChanged(object sender, EventArgs e)
        {
            this.SetLapTimeGraphValues();
        }
        private void nud_lapTimeGraphNumberOfLapsToShow_ValueChanged(object sender, EventArgs e)
        {
            this.SetLapTimeGraphValues();
        }

        private void button_saveFinalClassification_Click(object sender, EventArgs e)
        {
            //this.MainForm.FinalClassification();
            this.FinalClassification();
        }

        private void UpdateButtonLoadCustomDriverNames()
        {
            button_loadCustomDriverNames.Enabled = cb_customDriverNames.Checked;
            button_loadCustomDriverNames.BackColor = (Utility.CustomDriverNamesMap == null) ? System.Drawing.Color.Orange : System.Drawing.Color.LightGreen;
        }

        private void button_loadCustomDriverNames_Click(object sender, EventArgs e)
        {
            this.LoadCustomDriverNamesFromFile();
        }

    }
}
