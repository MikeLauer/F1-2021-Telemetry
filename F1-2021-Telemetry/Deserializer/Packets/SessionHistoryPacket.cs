using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimHanewich.Toolkit;

namespace Codemasters.F1_2021
{
    public class SessionHistoryPacket : Packet
    {
        public byte CarIndex { get; set; }
        public byte NumberOfLaps { get; set; } // Including current
        public byte NumberTyreStints { get; set; }
        
        public byte BestLapTimeLapNumber { get; set; }
        public byte BestSector1LapNumbers { get; set; }
        public byte BestSector2LapNumbers { get; set; }
        public byte BestSector3LapNumbers { get; set; }

        public LapHistoryData[] LapsHistoryData { get; set; }
        //public TyreStintHistoryData[] TyreStintsHistoryData { get; set; }


        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager BAM = new ByteArrayManager(bytes);
            base.LoadBytes(BAM.NextBytes(24));

            CarIndex = BAM.NextByte();

            NumberOfLaps = BAM.NextByte();

            NumberTyreStints = BAM.NextByte();

            BestLapTimeLapNumber = BAM.NextByte();

            BestSector1LapNumbers = BAM.NextByte();
            BestSector2LapNumbers = BAM.NextByte();
            BestSector3LapNumbers = BAM.NextByte();

            List<LapHistoryData> HDs = new List<LapHistoryData>();
            for(int i = 1; i <= 100; i++)
            {
                HDs.Add(LapHistoryData.Create(BAM.NextBytes(11)));
            }
            LapsHistoryData = HDs.ToArray();
        }

        public class LapHistoryData
        {
            public uint LapTimeInMs { get; set; }
            public ushort Sector1TimeInMs { get; set; }
            public ushort Sector2TimeInMs { get; set; }
            public ushort Sector3TimeInMs { get; set; }
            public bool LapValid { get; set; }

            public static LapHistoryData Create(byte[] bytes)
            {
                LapHistoryData ReturnInstance = new LapHistoryData();
                ByteArrayManager BAM = new ByteArrayManager(bytes);

                ReturnInstance.LapTimeInMs = BitConverter.ToUInt32(BAM.NextBytes(4), 0);

                ReturnInstance.Sector1TimeInMs = BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.Sector2TimeInMs = BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.Sector3TimeInMs = BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                int lapValid = BAM.NextByte() & 1;
                if (lapValid == 1)
                {
                    ReturnInstance.LapValid = true;
                }

                return ReturnInstance;
            }
        }
/*
        public class TyreStintHistoryData
        {
            public static TyreStintHistoryData Create(byte[] bytes)
            {
                TyreStintHistoryData ReturnInstance = new TyreStintHistoryData();

                return ReturnInstance;
            }
        }
*/
    }
}
