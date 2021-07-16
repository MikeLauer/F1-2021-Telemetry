
namespace F1_2021_Telemetry
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.cb_show_deltaBestInterval = new System.Windows.Forms.CheckBox();
            this.cb_show_lastSector = new System.Windows.Forms.CheckBox();
            this.cb_show_bestSector = new System.Windows.Forms.CheckBox();
            this.cb_show_status = new System.Windows.Forms.CheckBox();
            this.cb_show_positionDifference = new System.Windows.Forms.CheckBox();
            this.cb_show_pitStatus = new System.Windows.Forms.CheckBox();
            this.cb_show_tyre = new System.Windows.Forms.CheckBox();
            this.cb_show_gapLeader = new System.Windows.Forms.CheckBox();
            this.cb_show_gapInterval = new System.Windows.Forms.CheckBox();
            this.cb_show_deltaLastInterval = new System.Windows.Forms.CheckBox();
            this.cb_show_deltaBestLeader = new System.Windows.Forms.CheckBox();
            this.cb_show_lastLap = new System.Windows.Forms.CheckBox();
            this.cb_show_bestLap = new System.Windows.Forms.CheckBox();
            this.cb_show_lapNumber = new System.Windows.Forms.CheckBox();
            this.button_save = new System.Windows.Forms.Button();
            this.cb_show_timePenalties = new System.Windows.Forms.CheckBox();
            this.cb_highlightPlayer = new System.Windows.Forms.CheckBox();
            this.cb_liveTimings = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_performanceMode = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_show_deltaBestInterval
            // 
            this.cb_show_deltaBestInterval.AutoSize = true;
            this.cb_show_deltaBestInterval.Checked = true;
            this.cb_show_deltaBestInterval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_deltaBestInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_deltaBestInterval.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_deltaBestInterval.Location = new System.Drawing.Point(20, 88);
            this.cb_show_deltaBestInterval.Name = "cb_show_deltaBestInterval";
            this.cb_show_deltaBestInterval.Size = new System.Drawing.Size(134, 17);
            this.cb_show_deltaBestInterval.TabIndex = 77;
            this.cb_show_deltaBestInterval.Text = "Delta Best Lap Interval";
            this.cb_show_deltaBestInterval.UseVisualStyleBackColor = true;
            this.cb_show_deltaBestInterval.CheckedChanged += new System.EventHandler(this.cb_show_deltaBestInterval_CheckedChanged);
            // 
            // cb_show_lastSector
            // 
            this.cb_show_lastSector.AutoSize = true;
            this.cb_show_lastSector.Checked = true;
            this.cb_show_lastSector.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_lastSector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_lastSector.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_lastSector.Location = new System.Drawing.Point(205, 88);
            this.cb_show_lastSector.Name = "cb_show_lastSector";
            this.cb_show_lastSector.Size = new System.Drawing.Size(80, 17);
            this.cb_show_lastSector.TabIndex = 76;
            this.cb_show_lastSector.Text = "Last Sector";
            this.cb_show_lastSector.UseVisualStyleBackColor = true;
            this.cb_show_lastSector.CheckedChanged += new System.EventHandler(this.cb_show_lastSector_CheckedChanged);
            // 
            // cb_show_bestSector
            // 
            this.cb_show_bestSector.AutoSize = true;
            this.cb_show_bestSector.Checked = true;
            this.cb_show_bestSector.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_bestSector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_bestSector.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_bestSector.Location = new System.Drawing.Point(205, 65);
            this.cb_show_bestSector.Name = "cb_show_bestSector";
            this.cb_show_bestSector.Size = new System.Drawing.Size(81, 17);
            this.cb_show_bestSector.TabIndex = 75;
            this.cb_show_bestSector.Text = "Best Sector";
            this.cb_show_bestSector.UseVisualStyleBackColor = true;
            this.cb_show_bestSector.CheckedChanged += new System.EventHandler(this.cb_show_bestSector_CheckedChanged);
            // 
            // cb_show_status
            // 
            this.cb_show_status.AutoSize = true;
            this.cb_show_status.Checked = true;
            this.cb_show_status.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_status.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_status.Location = new System.Drawing.Point(206, 42);
            this.cb_show_status.Name = "cb_show_status";
            this.cb_show_status.Size = new System.Drawing.Size(56, 17);
            this.cb_show_status.TabIndex = 74;
            this.cb_show_status.Text = "Status";
            this.cb_show_status.UseVisualStyleBackColor = true;
            this.cb_show_status.CheckedChanged += new System.EventHandler(this.cb_show_status_CheckedChanged);
            // 
            // cb_show_positionDifference
            // 
            this.cb_show_positionDifference.AutoSize = true;
            this.cb_show_positionDifference.Checked = true;
            this.cb_show_positionDifference.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_positionDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_positionDifference.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_positionDifference.Location = new System.Drawing.Point(206, 136);
            this.cb_show_positionDifference.Name = "cb_show_positionDifference";
            this.cb_show_positionDifference.Size = new System.Drawing.Size(80, 17);
            this.cb_show_positionDifference.TabIndex = 73;
            this.cb_show_positionDifference.Text = "+/- Position";
            this.cb_show_positionDifference.UseVisualStyleBackColor = true;
            this.cb_show_positionDifference.CheckedChanged += new System.EventHandler(this.cb_show_positionDifference_CheckedChanged);
            // 
            // cb_show_pitStatus
            // 
            this.cb_show_pitStatus.AutoSize = true;
            this.cb_show_pitStatus.Checked = true;
            this.cb_show_pitStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_pitStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_pitStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_pitStatus.Location = new System.Drawing.Point(206, 19);
            this.cb_show_pitStatus.Name = "cb_show_pitStatus";
            this.cb_show_pitStatus.Size = new System.Drawing.Size(86, 17);
            this.cb_show_pitStatus.TabIndex = 72;
            this.cb_show_pitStatus.Text = "Pit/DNF/DQ";
            this.cb_show_pitStatus.UseVisualStyleBackColor = true;
            this.cb_show_pitStatus.CheckedChanged += new System.EventHandler(this.cb_show_pitStatus_CheckedChanged);
            // 
            // cb_show_tyre
            // 
            this.cb_show_tyre.AutoSize = true;
            this.cb_show_tyre.Checked = true;
            this.cb_show_tyre.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_tyre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_tyre.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_tyre.Location = new System.Drawing.Point(206, 113);
            this.cb_show_tyre.Name = "cb_show_tyre";
            this.cb_show_tyre.Size = new System.Drawing.Size(47, 17);
            this.cb_show_tyre.TabIndex = 71;
            this.cb_show_tyre.Text = "Tyre";
            this.cb_show_tyre.UseVisualStyleBackColor = true;
            this.cb_show_tyre.CheckedChanged += new System.EventHandler(this.cb_show_tyre_CheckedChanged);
            // 
            // cb_show_gapLeader
            // 
            this.cb_show_gapLeader.AutoSize = true;
            this.cb_show_gapLeader.Checked = true;
            this.cb_show_gapLeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_gapLeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_gapLeader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_gapLeader.Location = new System.Drawing.Point(21, 182);
            this.cb_show_gapLeader.Name = "cb_show_gapLeader";
            this.cb_show_gapLeader.Size = new System.Drawing.Size(82, 17);
            this.cb_show_gapLeader.TabIndex = 70;
            this.cb_show_gapLeader.Text = "Gap Leader";
            this.cb_show_gapLeader.UseVisualStyleBackColor = true;
            this.cb_show_gapLeader.CheckedChanged += new System.EventHandler(this.cb_show_gapLeader_CheckedChanged);
            // 
            // cb_show_gapInterval
            // 
            this.cb_show_gapInterval.AutoSize = true;
            this.cb_show_gapInterval.Checked = true;
            this.cb_show_gapInterval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_gapInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_gapInterval.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_gapInterval.Location = new System.Drawing.Point(21, 159);
            this.cb_show_gapInterval.Name = "cb_show_gapInterval";
            this.cb_show_gapInterval.Size = new System.Drawing.Size(84, 17);
            this.cb_show_gapInterval.TabIndex = 69;
            this.cb_show_gapInterval.Text = "Gap Interval";
            this.cb_show_gapInterval.UseVisualStyleBackColor = true;
            this.cb_show_gapInterval.CheckedChanged += new System.EventHandler(this.cb_show_gapInterval_CheckedChanged);
            // 
            // cb_show_deltaLastInterval
            // 
            this.cb_show_deltaLastInterval.AutoSize = true;
            this.cb_show_deltaLastInterval.Checked = true;
            this.cb_show_deltaLastInterval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_deltaLastInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_deltaLastInterval.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_deltaLastInterval.Location = new System.Drawing.Point(21, 136);
            this.cb_show_deltaLastInterval.Name = "cb_show_deltaLastInterval";
            this.cb_show_deltaLastInterval.Size = new System.Drawing.Size(133, 17);
            this.cb_show_deltaLastInterval.TabIndex = 68;
            this.cb_show_deltaLastInterval.Text = "Delta Last Lap Interval";
            this.cb_show_deltaLastInterval.UseVisualStyleBackColor = true;
            this.cb_show_deltaLastInterval.CheckedChanged += new System.EventHandler(this.cb_show_deltaLastInterval_CheckedChanged);
            // 
            // cb_show_deltaBestLeader
            // 
            this.cb_show_deltaBestLeader.AutoSize = true;
            this.cb_show_deltaBestLeader.Checked = true;
            this.cb_show_deltaBestLeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_deltaBestLeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_deltaBestLeader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_deltaBestLeader.Location = new System.Drawing.Point(20, 65);
            this.cb_show_deltaBestLeader.Name = "cb_show_deltaBestLeader";
            this.cb_show_deltaBestLeader.Size = new System.Drawing.Size(96, 17);
            this.cb_show_deltaBestLeader.TabIndex = 67;
            this.cb_show_deltaBestLeader.Text = "Delta Best Lap";
            this.cb_show_deltaBestLeader.UseVisualStyleBackColor = true;
            this.cb_show_deltaBestLeader.CheckedChanged += new System.EventHandler(this.cb_show_deltaBestLeader_CheckedChanged);
            // 
            // cb_show_lastLap
            // 
            this.cb_show_lastLap.AutoSize = true;
            this.cb_show_lastLap.Checked = true;
            this.cb_show_lastLap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_lastLap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_lastLap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_lastLap.Location = new System.Drawing.Point(21, 113);
            this.cb_show_lastLap.Name = "cb_show_lastLap";
            this.cb_show_lastLap.Size = new System.Drawing.Size(67, 17);
            this.cb_show_lastLap.TabIndex = 66;
            this.cb_show_lastLap.Text = "Last Lap";
            this.cb_show_lastLap.UseVisualStyleBackColor = true;
            this.cb_show_lastLap.CheckedChanged += new System.EventHandler(this.cb_show_lastLap_CheckedChanged);
            // 
            // cb_show_bestLap
            // 
            this.cb_show_bestLap.AutoSize = true;
            this.cb_show_bestLap.Checked = true;
            this.cb_show_bestLap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_bestLap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_bestLap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_bestLap.Location = new System.Drawing.Point(20, 42);
            this.cb_show_bestLap.Name = "cb_show_bestLap";
            this.cb_show_bestLap.Size = new System.Drawing.Size(68, 17);
            this.cb_show_bestLap.TabIndex = 65;
            this.cb_show_bestLap.Text = "Best Lap";
            this.cb_show_bestLap.UseVisualStyleBackColor = true;
            this.cb_show_bestLap.CheckedChanged += new System.EventHandler(this.cb_show_bestLap_CheckedChanged);
            // 
            // cb_show_lapNumber
            // 
            this.cb_show_lapNumber.AutoSize = true;
            this.cb_show_lapNumber.Checked = true;
            this.cb_show_lapNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_lapNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_lapNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_lapNumber.Location = new System.Drawing.Point(20, 19);
            this.cb_show_lapNumber.Name = "cb_show_lapNumber";
            this.cb_show_lapNumber.Size = new System.Drawing.Size(84, 17);
            this.cb_show_lapNumber.TabIndex = 64;
            this.cb_show_lapNumber.Text = "Lap Number";
            this.cb_show_lapNumber.UseVisualStyleBackColor = true;
            this.cb_show_lapNumber.CheckedChanged += new System.EventHandler(this.cb_show_lapNumber_CheckedChanged);
            // 
            // button_save
            // 
            this.button_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_save.Location = new System.Drawing.Point(271, 427);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(121, 36);
            this.button_save.TabIndex = 78;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // cb_show_timePenalties
            // 
            this.cb_show_timePenalties.AutoSize = true;
            this.cb_show_timePenalties.Checked = true;
            this.cb_show_timePenalties.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_show_timePenalties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_show_timePenalties.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_show_timePenalties.Location = new System.Drawing.Point(206, 159);
            this.cb_show_timePenalties.Name = "cb_show_timePenalties";
            this.cb_show_timePenalties.Size = new System.Drawing.Size(95, 17);
            this.cb_show_timePenalties.TabIndex = 80;
            this.cb_show_timePenalties.Text = "Time Penalties";
            this.cb_show_timePenalties.UseVisualStyleBackColor = true;
            this.cb_show_timePenalties.CheckedChanged += new System.EventHandler(this.cb_show_timePenalties_CheckedChanged);
            // 
            // cb_highlightPlayer
            // 
            this.cb_highlightPlayer.AutoSize = true;
            this.cb_highlightPlayer.Checked = true;
            this.cb_highlightPlayer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_highlightPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_highlightPlayer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_highlightPlayer.Location = new System.Drawing.Point(21, 42);
            this.cb_highlightPlayer.Name = "cb_highlightPlayer";
            this.cb_highlightPlayer.Size = new System.Drawing.Size(141, 17);
            this.cb_highlightPlayer.TabIndex = 81;
            this.cb_highlightPlayer.Text = "Highlight Human Players";
            this.cb_highlightPlayer.UseVisualStyleBackColor = true;
            this.cb_highlightPlayer.CheckedChanged += new System.EventHandler(this.cb_highlightPlayer_CheckedChanged);
            // 
            // cb_liveTimings
            // 
            this.cb_liveTimings.AutoSize = true;
            this.cb_liveTimings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_liveTimings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_liveTimings.Location = new System.Drawing.Point(21, 19);
            this.cb_liveTimings.Name = "cb_liveTimings";
            this.cb_liveTimings.Size = new System.Drawing.Size(114, 17);
            this.cb_liveTimings.TabIndex = 83;
            this.cb_liveTimings.Text = "Live Sector Timing";
            this.cb_liveTimings.UseVisualStyleBackColor = true;
            this.cb_liveTimings.CheckedChanged += new System.EventHandler(this.cb_liveTimings_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_liveTimings);
            this.groupBox1.Controls.Add(this.cb_highlightPlayer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 84);
            this.groupBox1.TabIndex = 84;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_performanceMode);
            this.groupBox2.Location = new System.Drawing.Point(12, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 91);
            this.groupBox2.TabIndex = 85;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resources";
            // 
            // cb_performanceMode
            // 
            this.cb_performanceMode.AutoSize = true;
            this.cb_performanceMode.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cb_performanceMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_performanceMode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_performanceMode.Location = new System.Drawing.Point(20, 19);
            this.cb_performanceMode.Name = "cb_performanceMode";
            this.cb_performanceMode.Size = new System.Drawing.Size(322, 43);
            this.cb_performanceMode.TabIndex = 82;
            this.cb_performanceMode.Text = "Performance Mode:\r\nDisables colours in the leaderboard table and therby decreases" +
    "\r\nCPU load by a few percent";
            this.cb_performanceMode.UseVisualStyleBackColor = true;
            this.cb_performanceMode.CheckedChanged += new System.EventHandler(this.cb_performanceMode_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_show_lapNumber);
            this.groupBox3.Controls.Add(this.cb_show_bestLap);
            this.groupBox3.Controls.Add(this.cb_show_timePenalties);
            this.groupBox3.Controls.Add(this.cb_show_gapInterval);
            this.groupBox3.Controls.Add(this.cb_show_bestSector);
            this.groupBox3.Controls.Add(this.cb_show_lastSector);
            this.groupBox3.Controls.Add(this.cb_show_deltaBestInterval);
            this.groupBox3.Controls.Add(this.cb_show_status);
            this.groupBox3.Controls.Add(this.cb_show_deltaBestLeader);
            this.groupBox3.Controls.Add(this.cb_show_tyre);
            this.groupBox3.Controls.Add(this.cb_show_pitStatus);
            this.groupBox3.Controls.Add(this.cb_show_deltaLastInterval);
            this.groupBox3.Controls.Add(this.cb_show_positionDifference);
            this.groupBox3.Controls.Add(this.cb_show_lastLap);
            this.groupBox3.Controls.Add(this.cb_show_gapLeader);
            this.groupBox3.Location = new System.Drawing.Point(12, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(380, 222);
            this.groupBox3.TabIndex = 86;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Table Column Visibility";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 475);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSettings_FormClosed);
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_show_deltaBestInterval;
        private System.Windows.Forms.CheckBox cb_show_lastSector;
        private System.Windows.Forms.CheckBox cb_show_bestSector;
        private System.Windows.Forms.CheckBox cb_show_status;
        private System.Windows.Forms.CheckBox cb_show_positionDifference;
        private System.Windows.Forms.CheckBox cb_show_pitStatus;
        private System.Windows.Forms.CheckBox cb_show_tyre;
        private System.Windows.Forms.CheckBox cb_show_gapLeader;
        private System.Windows.Forms.CheckBox cb_show_gapInterval;
        private System.Windows.Forms.CheckBox cb_show_deltaLastInterval;
        private System.Windows.Forms.CheckBox cb_show_deltaBestLeader;
        private System.Windows.Forms.CheckBox cb_show_lastLap;
        private System.Windows.Forms.CheckBox cb_show_bestLap;
        private System.Windows.Forms.CheckBox cb_show_lapNumber;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.CheckBox cb_show_timePenalties;
        private System.Windows.Forms.CheckBox cb_highlightPlayer;
        private System.Windows.Forms.CheckBox cb_liveTimings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_performanceMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}