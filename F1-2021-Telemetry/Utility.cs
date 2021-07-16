using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_2021_Telemetry
{
    class Utility
    {
        public static String formatTimeToMinutes(float time, bool withMs = true, bool baseTimeInSeconds = false)
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

        public static String formatTimeToSeconds(float time, bool withSign = false)
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
    }

}
