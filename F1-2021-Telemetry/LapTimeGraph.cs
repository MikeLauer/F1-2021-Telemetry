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

        private FormMain MainForm;

        private SessionHistoryPacket[] SessionHistoryPackets;
        private LeaderboardDriver Driver;

        public int NumberDriversInFront = 1;
        public int NumberDriversBehind = 1;
        public int NumberOfLapsToShow = 10;

        public LapTimeGraph(FormMain parent, Chart lapTimeChart)
        {
            this.MainForm = parent;
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

            int numberOfLapsToShow = (this.Driver.CurrentLapNumber <= this.NumberOfLapsToShow) ? this.Driver.CurrentLapNumber - 1 : this.NumberOfLapsToShow;

            List<double> lapTimesSelf = new List<double>(); // List of own lap times
            List<List<double>> lapTimesFront = new List<List<double>>(); // List of lists of lap times
            List<List<double>> lapTimesBehind = new List<List<double>>(); // List of lists of lap times


            List<LeaderboardDriver> driversInFront = new List<LeaderboardDriver>(); // List of drivers
            List<LeaderboardDriver> driversBehind = new List<LeaderboardDriver>(); // List of drivers

            // Drivers in front
            for (int i = 0; i < this.NumberDriversInFront; i++)
            {
                LeaderboardDriver driverFront_i = null;
                if (this.Driver.CarPosition > 1 + i)
                {
                    driverFront_i = this.MainForm.LatestLeaderboard.DriverData[this.Driver.CarPosition - 2 - i];
                    if (driverFront_i != null && this.SessionHistoryPackets[driverFront_i.Index] != null)
                    {
                        driversInFront.Add(driverFront_i);
                        List<double> lapTimes = new List<double>();
                        lapTimesFront.Add(lapTimes);
                    }
                }
            }

            // Drivers behind
            for (int i = 0; i < this.NumberDriversBehind; i++)
            {
                LeaderboardDriver driverBehind_i = null;
                if (this.Driver.CarPosition < 20 - i)
                {
                    driverBehind_i = this.MainForm.LatestLeaderboard.DriverData[this.Driver.CarPosition + i];
                    if (driverBehind_i != null && this.SessionHistoryPackets[driverBehind_i.Index] != null)
                    {
                        driversBehind.Add(driverBehind_i);
                        List<double> lapTimes = new List<double>();
                        lapTimesBehind.Add(lapTimes);
                    }
                }
            }

            // Old code
            /*LeaderboardDriver driverFront = null;
            if (this.Driver.CarPosition > 1)
                driverFront = this.Parent.LatestLeaderboard.DriverData[this.Driver.CarPosition - 2];

            LeaderboardDriver driverBehind = null;
            if (this.Driver.CarPosition < 20)
                driverBehind = this.Parent.LatestLeaderboard.DriverData[this.Driver.CarPosition];

            bool driverFrontValid = driverFront != null && this.SessionHistoryPackets[driverFront.Index] != null; // whether a driver in front exists (and its data)
            bool driverBehindValid = driverBehind != null && this.SessionHistoryPackets[driverBehind.Index] != null; // whether a driver behind exists (and its data)*/

            int lapStartIndex = this.Driver.CurrentLapNumber > numberOfLapsToShow ? this.Driver.CurrentLapNumber - numberOfLapsToShow - 1 : 0; // Current lap number - number of laps to show       

            // Insert dummy values as offset, so the laps are visible in the right spot
            for (int i = 0; i < lapStartIndex; i++)
            {
                lapTimesSelf.Add(0);
                for (int k = 0; k < lapTimesFront.Count; k++)
                    lapTimesFront[k].Add(0);
                for (int k = 0; k < lapTimesBehind.Count; k++)
                    lapTimesBehind[k].Add(0);
            }

            double min = 1000; // min lap time
            double max = 0; // max lap time

            for (int i = lapStartIndex; i < lapStartIndex + numberOfLapsToShow; i++) // for each lap that will be visible
            {
                // Self
                double lapTime = this.SessionHistoryPackets[this.Driver.Index].LapsHistoryData[i].LapTimeInMs / 1000.0; // Lap time in seconds
                if (lapTime != 0)
                    lapTimesSelf.Add(lapTime); // Add own time to list

                // Keep track of min/max
                if (lapTime > max)
                {
                    max = lapTime;
                }
                if (lapTime < min && lapTime > 0)
                {
                    min = lapTime;
                }


                // Driver in front
                for (int k = 0; k < lapTimesFront.Count; k++)
                {
                    LeaderboardDriver driver = driversInFront[k];
                    lapTime = this.SessionHistoryPackets[driver.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesFront[k].Add(lapTime); // Add time to list

                    // Keep track of min/max
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
                for (int k = 0; k < lapTimesBehind.Count; k++)
                {
                    LeaderboardDriver driver = driversBehind[k];
                    lapTime = this.SessionHistoryPackets[driver.Index].LapsHistoryData[i].LapTimeInMs / 1000.0;
                    if (lapTime != 0)
                        lapTimesBehind[k].Add(lapTime); // Add time to list

                    // Keep track of min/max
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
            //selfSeries.Color = this.Driver.TeamColor;
            selfSeries.Color = System.Drawing.Color.Fuchsia;
            selfSeries.BorderWidth = 6; // Own line thicker than the others

            // Create series for driver behind
            List<Series> allDriverBehindSeries = new List<Series>();
            for (int i = 0; i < driversBehind.Count; i++)
            {
                Series driverBehindSeries = new Series("Behind"+i);
                LeaderboardDriver driver = driversBehind[i];
                driverBehindSeries.Points.DataBindY(lapTimesBehind[i].ToArray());
                driverBehindSeries.ChartType = SeriesChartType.FastLine;
                driverBehindSeries.Color = driver.TeamColor;
                driverBehindSeries.BorderWidth = 3;
                driverBehindSeries.BorderDashStyle = ChartDashStyle.Dash;
                allDriverBehindSeries.Add(driverBehindSeries);
            }

            // Create series for driver in front
            List<Series> allDriverInFrontSeries = new List<Series>();
            for (int i = 0; i < driversInFront.Count; i++)
            {
                Series driverFrontSeries = new Series("Front"+i);
                LeaderboardDriver driver = driversInFront[i];
                driverFrontSeries.Points.DataBindY(lapTimesFront[i].ToArray());
                driverFrontSeries.ChartType = SeriesChartType.FastLine;
                driverFrontSeries.Color = driver.TeamColor;
                driverFrontSeries.BorderWidth = 3;
                allDriverInFrontSeries.Add(driverFrontSeries);
            }

            lapTimeChart.Invoke((MethodInvoker)delegate ()
            {
                // Add each series to the chart
                lapTimeChart.Series.Clear();
                allDriverBehindSeries.ForEach(driverBehindSeries => lapTimeChart.Series.Add(driverBehindSeries));
                lapTimeChart.Series.Add(selfSeries);
                allDriverInFrontSeries.ForEach(driversInFrontSeries => lapTimeChart.Series.Add(driversInFrontSeries));

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
                lapTimeChart.ChartAreas[0].AxisX.Maximum = lapStartIndex + numberOfLapsToShow + 1;
            });
        }
    }
}
