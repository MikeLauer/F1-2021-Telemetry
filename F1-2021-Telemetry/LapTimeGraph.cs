using Codemasters.F1_2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace F1_2021_Telemetry
{
    class LapTimeGraph
    {
        private System.Timers.Timer UpdateGraphTimer;
        private Chart lapTimeChart;

        private FormMain Parent;

        private SessionHistoryPacket[] SessionHistoryPackets;
        private LeaderboardDriver Driver;

        public LapTimeGraph(FormMain parent, Chart lapTimeChart)
        {
            this.Parent = parent;
            this.lapTimeChart = lapTimeChart;

            UpdateGraphTimer = new System.Timers.Timer();
            UpdateGraphTimer.Interval = 500;
            UpdateGraphTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateGraph);
            UpdateGraphTimer.Start();
        }

        public void UpdateData(SessionHistoryPacket[] sessionHistoryPackets, LeaderboardDriver driver)
        {
            this.SessionHistoryPackets = sessionHistoryPackets;
            this.Driver = driver;
        }

        private void UpdateGraph(object sender, EventArgs e)
        {
            if (this.SessionHistoryPackets == null || this.SessionHistoryPackets[this.Driver.Index] == null)
            {
                return; // If no history data
            }

            int numberOfLapsToShow = 10;
            if(lapTimeChart.Width > 400)
            {
                numberOfLapsToShow = 15;
            }

            List<double> lapTimesSelf = new List<double>();
            List<double> lapTimesFront = new List<double>();
            List<double> lapTimesBehind = new List<double>();


            LeaderboardDriver driverFront = null;
            if (this.Driver.CarPosition > 1)
                driverFront = this.Parent.LatestLeaderboard.DriverData[this.Driver.CarPosition - 2];

            LeaderboardDriver driverBehind = null;
            if (this.Driver.CarPosition < 20)
                driverBehind = this.Parent.LatestLeaderboard.DriverData[this.Driver.CarPosition];

            bool driverFrontValid = driverFront != null && this.SessionHistoryPackets[driverFront.Index] != null; // whether a driver in front exists (and its data)
            bool driverBehindValid = driverBehind != null && this.SessionHistoryPackets[driverBehind.Index] != null; // whether a driver behind exists (and its data)

            int lapStartIndex = this.Driver.CurrentLapNumber > numberOfLapsToShow ? this.Driver.CurrentLapNumber - numberOfLapsToShow - 1 : 0; // Current lap number - 10

            for (int i = 0; i < lapStartIndex; i++) // Offset of lines in graph so the laps are visibile and in the right spot
            {
                lapTimesSelf.Add(0);
                lapTimesFront.Add(0);
                lapTimesBehind.Add(0);
            }

            double min = 1000; // min lap time
            double max = 0; // max lap time

            for (int i = lapStartIndex; i < lapStartIndex + numberOfLapsToShow; i++) // for each lap
            {
                // Self
                double lapTime = this.SessionHistoryPackets[this.Driver.Index].LapsHistoryData[i].LapTimeInMs / 1000.0; // Lap time in seconds
                if (lapTime != 0)
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
                    lapTime = this.SessionHistoryPackets[driverFront.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesFront.Add(lapTime); // Add time to list
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
                    lapTime = this.SessionHistoryPackets[driverBehind.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesBehind.Add(lapTime); // Add time to list
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
            selfSeries.Color = this.Driver.TeamColor;
            selfSeries.BorderWidth = 6; // Own line thicker than the others

            // Create series for driver in front
            Series driverFrontSeries = new Series("Front");
            if (driverFrontValid)
            {
                driverFrontSeries.Points.DataBindY(lapTimesFront.ToArray());
                driverFrontSeries.ChartType = SeriesChartType.FastLine;
                driverFrontSeries.Color = driverFront.TeamColor;
                driverFrontSeries.BorderWidth = 3;
            }

            // Create series for driver behind
            Series driverBehindSeries = new Series("Behind");
            if (driverBehindValid)
            {
                driverBehindSeries.Points.DataBindY(lapTimesBehind.ToArray());
                driverBehindSeries.ChartType = SeriesChartType.FastLine;
                driverBehindSeries.Color = driverBehind.TeamColor;
                driverBehindSeries.BorderWidth = 3;
            }

            lapTimeChart.Invoke((MethodInvoker)delegate ()
            {
                // Add each series to the chart
                lapTimeChart.Series.Clear();
                lapTimeChart.Series.Add(selfSeries);
                lapTimeChart.Series.Add(driverFrontSeries);
                lapTimeChart.Series.Add(driverBehindSeries);

                min = Math.Floor(min);
                max = Math.Ceiling(max);

                // Additional styling
                //lapTimeChart.ResetAutoValues();
                lapTimeChart.Titles.Clear();
                lapTimeChart.ChartAreas[0].AxisY.Interval = Math.Round((max - min) / 5.0, 0); // five steps on y axis
                lapTimeChart.ChartAreas[0].AxisX.Interval = 1; // 1 lap steps
                lapTimeChart.ChartAreas[0].AxisY.Minimum = min - 1;
                lapTimeChart.ChartAreas[0].AxisY.Maximum = max + 1;
                lapTimeChart.ChartAreas[0].AxisX.Minimum = lapStartIndex + 1;
                lapTimeChart.ChartAreas[0].AxisX.Maximum = lapStartIndex + numberOfLapsToShow;
            });
        }
    }
}
