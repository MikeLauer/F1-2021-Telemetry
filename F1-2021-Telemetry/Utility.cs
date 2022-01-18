using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace F1_2021_Telemetry
{
    class Utility
    {
        public static Dictionary<byte, string> CustomDriverNamesMap = null;

        public static string formatTimeToMinutes(float time, bool withMs = true, bool baseTimeInSeconds = false)
        {
            TimeSpan format;

            if (baseTimeInSeconds)
                format = TimeSpan.FromSeconds(time);
            else format = TimeSpan.FromMilliseconds(time);

            string timeFormatted;
            if (withMs)
            {
                timeFormatted = format.ToString(@"mm\:ss\.fff");
            }
            else
            {
                timeFormatted = format.ToString(@"mm\:ss");
            }
            return timeFormatted;
        }

        public static string formatTimeToSeconds(float time, bool withSign = false)
        {
            string timeFormatted = "";

            if (time != 0)
            {
                TimeSpan format = TimeSpan.FromMilliseconds(time);

                if (time < 60000)
                    timeFormatted = format.ToString(@"s\.fff");
                else timeFormatted = format.ToString(@"m\:ss");

                if (withSign)
                {
                    if (time > 0) timeFormatted = "+" + timeFormatted;
                    else if (time < 0) timeFormatted = "-" + timeFormatted;
                }
            }

            return timeFormatted;
        }

        public void SetCustomDriverNamesMap(Dictionary<byte, string> customDriverNamesMap)
        {
            Utility.CustomDriverNamesMap = customDriverNamesMap;
        }

        public static string GetCustomDriverNameFromRaceNumber(byte raceNumber, string alternativeName = null)
        {
            string returnName;
            if(Utility.CustomDriverNamesMap.TryGetValue(raceNumber, out returnName) == false)
            {
                returnName = (alternativeName == null) ? "N.A." : alternativeName;
            }
            
            return returnName;
        }

        public static void OutputFinalClassification(SaveFileDialog saveFileDialog, Codemasters.F1_2021.FinalClassificationPacket finalClassificationPacket, Codemasters.F1_2021.ParticipantPacket participantPacket, Codemasters.F1_2021.SessionPacket sessionPacket, Dictionary<byte, string> equalityLeagueDriverNameMap = null)
        {
            
        }

        public class FinalClassificationDriver
        {
            public string Name { get; set; }
            public byte RaceNumber { get; set; }

            public byte Position { get; set; }
            public byte NumLaps { get; set; }
            public byte GridPosition { get; set; }
            public byte Points { get; set; }
            public byte NumPitStops { get; set; }
            public string ResultStatus { get; set; }
            public uint BestLapTimeInMS { get; set; }
            public double TotalRaceTimeInSec { get; set; }
            public byte PenaltiesTime { get; set; }
            public byte NumPenalties { get; set; }
            public byte NumTyreStins { get; set; }

        }

        public class FinalClassification
        {
            public string Track { get; set; }
            public FinalClassificationDriver[] Drivers { get; set; }
        }
    }

}
