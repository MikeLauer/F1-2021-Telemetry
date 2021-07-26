using Codemasters.F1_2021;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_2021_Telemetry
{
    /// <summary>
    /// Leaderboard class. Contains of 20 drivers each full with its own data
    /// </summary>
    public class Leaderboard
    {
        public LeaderboardDriver[] DriverData;

        public uint TheoredicalBestLap = 0;

        public Leaderboard()
        {
            Clear();
        }

        public void Clear()
        {
            DriverData = new LeaderboardDriver[20];
        }

        /// <summary>
        /// Put driver in array at certain position.
        /// 
        /// Important: Race position, not array position!
        /// </summary>
        /// <param name="racePosition">1-20</param>
        /// <param name="driver"></param>
        public void SetDriverToPosition(int racePosition, LeaderboardDriver driver)
        {
            if(racePosition > 0 && racePosition <= 20) 
                DriverData[racePosition-1] = driver;
        }

        public LeaderboardDriver getDriver(int index)
        {
            return DriverData[index];
        }
    }

    /// <summary>
    /// Represents a driver in the leaderboard. Holds all data about a driver
    /// </summary>
    public class LeaderboardDriver
    {
        static readonly Color[] ForeColorCoding = { Color.White, Color.Yellow, Color.LimeGreen, Color.Purple };
        static readonly Color[] BackColorCodingEven = { Color.Black, Color.Yellow, Color.LimeGreen, Color.Purple };
        static readonly Color[] BackColorCodingOdd = { Color.FromArgb(50,50,50) , Color.Yellow, Color.LimeGreen, Color.Purple };
        static readonly Color[] TeamColors = { Color.FromArgb(00, 210, 190), Color.FromArgb(220, 0, 0), Color.FromArgb(6, 0, 239), Color.FromArgb(0, 90, 255), Color.FromArgb(0, 111, 98), Color.FromArgb(0, 144, 255), Color.FromArgb(43, 69, 98), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 135, 0), Color.FromArgb(144, 0, 0) };

        public byte Index; // Index of the original order

        public Color TeamColor;

        public byte CarPosition; // Race position
        public string DriverName;
        public byte CurrentLapNumber = 0;

        public uint BestLapTime = 0;
        public Color BestLapTimeColor;        

        public uint LastLapTime = 0;
        public Color LastLapTimeColor; 

        public int DeltaBestLap;
        public int DeltaBestLapInterval;
        public int DeltaLastLapInterval;
        public uint GapInterval;
        public bool DrsAllowed = false;
        public uint GapLeader;        

        public string Tyre;
        public string PitStatus;
        public string PositionDifferenceToStartgrid;
        public string DriverStatus;

        public uint Sector1Best = 0;
        public Color Sector1BestColor; 
        public uint Sector1Last = 0;
        public Color Sector1LastColor; 

        public uint Sector2Best = 0;
        public Color Sector2BestColor;
        public uint Sector2Last = 0;
        public Color Sector2LastColor;

        public uint Sector3Best = 0;
        public Color Sector3BestColor;
        public uint Sector3Last = 0;
        public Color Sector3LastColor;

        public bool HasOvertaken;
        public byte OvertakenPosition;

        public float LapDistance = 0;

        public byte TimePenaltiesInSeconds = 0;       

        public void SetTeamColor(int teamIndex)
        {
            if (teamIndex < TeamColors.Length)
                TeamColor = TeamColors[teamIndex];
            else TeamColor = Color.Gray;
        }
        public string GetBestLapTimeFormatted()
        {
            return Utility.formatTimeToMinutes(BestLapTime);
        }

        public void SetBestAndLastLapTime(uint bestLapTime, uint lastLapTime)
        {
            this.BestLapTime = bestLapTime;
            this.LastLapTime = lastLapTime;
            this.SetBestLapTimeColor(LeaderboardFlags.White); // May change later
            if(bestLapTime == lastLapTime)
            {
                this.SetLastLapTimeColor(LeaderboardFlags.Green);
            } else
            {
                this.SetLastLapTimeColor(LeaderboardFlags.Yellow);
            }
        }
        public void SetBestLapTimeColor(LeaderboardFlags flag)
        {
            Color color;
            if (CarPosition % 2 != 0)
                color = BackColorCodingEven[(int)flag];
            else color = BackColorCodingOdd[(int)flag];
            BestLapTimeColor = color;
        }
        public void SetLastLapTimeColor(LeaderboardFlags flag)
        {
            LastLapTimeColor = ForeColorCoding[(int)flag];
        }
        public string GetLastLapTimeFormatted()
        {
            return Utility.formatTimeToMinutes(LastLapTime);
        }


        public string getGapInterval()
        {
            return Utility.formatTimeToSeconds(GapInterval, true);
        }
        public string getGapLeader()
        {
            return Utility.formatTimeToSeconds(GapLeader, true);
        }

        public void SetSector1BestColor(LeaderboardFlags flag)
        {
            Color color;
            if (CarPosition % 2 != 0)
                color = BackColorCodingEven[(int)flag];
            else color = BackColorCodingOdd[(int)flag];
            Sector1BestColor = color;
        }
        public void SetSector1LastColor(LeaderboardFlags flag)
        {
            Sector1LastColor = ForeColorCoding[(int)flag];
        }
        public void SetSector2BestColor(LeaderboardFlags flag)
        {
            Color color;
            if (CarPosition % 2 != 0)
                color = BackColorCodingEven[(int)flag];
            else color = BackColorCodingOdd[(int)flag];
            Sector2BestColor = color;
        }
        public void SetSector2LastColor(LeaderboardFlags flag)
        {
            Sector2LastColor = ForeColorCoding[(int)flag];
        }
        public void SetSector3BestColor(LeaderboardFlags flag)
        {
            Color color;
            if (CarPosition % 2 != 0)
                color = BackColorCodingEven[(int)flag];
            else color = BackColorCodingOdd[(int)flag];
            Sector3BestColor = color;
        }
        public void SetSector3LastColor(LeaderboardFlags flag)
        {
            Sector3LastColor = ForeColorCoding[(int)flag];
        }

        public void SetDefaultBestSectorColours()
        {
            this.SetSector1BestColor(LeaderboardFlags.White);
            this.SetSector2BestColor(LeaderboardFlags.White);
            this.SetSector3BestColor(LeaderboardFlags.White);
        }


        /// <summary>
        /// Convert driver data into an array that can later be inserted into a datagridview for example
        /// </summary>
        /// <returns></returns>
        public Object[] getDataAsArray()
        {
            Object[] data = new object[22];

            data[0] = "";
            data[1] = CarPosition;
            data[2] = DriverName;
            data[3] = CurrentLapNumber;
            data[4] = GetBestLapTimeFormatted();
            data[5] = Utility.formatTimeToSeconds(DeltaBestLap, true);
            data[6] = Utility.formatTimeToSeconds(DeltaBestLapInterval, true);
            data[7] = GetLastLapTimeFormatted();
            data[8] = Utility.formatTimeToSeconds(DeltaLastLapInterval, true);
            data[9] = (this.PitStatus != "DNF" && this.PitStatus != "DSQ" && this.PitStatus != "Retired") ? getGapInterval() : "";
            data[10] = (this.PitStatus != "DNF" && this.PitStatus != "DSQ" && this.PitStatus != "Retired") ? getGapLeader() : "";
            data[11] = PitStatus;
            data[12] = DriverStatus;
            data[13] = Utility.formatTimeToSeconds(Sector1Best);
            data[14] = (Sector1Last == 0) ? "" : Utility.formatTimeToSeconds(Sector1Last);
            data[15] = Utility.formatTimeToSeconds(Sector2Best);
            data[16] = (Sector2Last == 0) ? "" : Utility.formatTimeToSeconds(Sector2Last);
            data[17] = Utility.formatTimeToSeconds(Sector3Best); ;
            data[18] = (Sector3Last == 0) ? "" : Utility.formatTimeToSeconds(Sector3Last);
            data[19] = Tyre;
            data[20] = PositionDifferenceToStartgrid;
            data[21] = TimePenaltiesInSeconds;

            return data;
        }
    }


    /// <summary>
    /// Flags and their index for the leaderboard
    /// </summary>
    public enum LeaderboardFlags
    {
        White = 0,
        Yellow = 1,
        Green = 2,
        Purple = 3
    }    
}
