using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            cb_liveTimings.Checked = this.MainForm.getLiveTimingEnabled();

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

            if (this.MainForm.HighLightOtherPlayers) rb_playerHighlight_everyone.Checked = true;
            else if (this.MainForm.HighlightOwnPlayer) rb_playerHighlight_onlyMe.Checked = true;
            else rb_playerHighlight_off.Checked = true;

            cb_performanceMode.Checked = this.MainForm.PerformanceMode;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
