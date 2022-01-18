using System;
using System.Collections.Generic;
using TimHanewich.Toolkit;

namespace Codemasters.F1_2021
{
    public class FinalClassificationPacket : Packet
    {

        public FinalClassificationData[] FieldDriverData { get; set; }

        public byte NumCars { get; set; }

        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager BAM = new ByteArrayManager(bytes);

            //Get header
            base.LoadBytes(BAM.NextBytes(24));

            this.NumCars = BAM.NextByte();


            //Get the next 20 data packages
            List<FinalClassificationData> LDs = new List<FinalClassificationData>();
            int t = 1;
            for (t = 1; t <= this.NumCars; t++)
            {
                LDs.Add(FinalClassificationData.Create(BAM.NextBytes(37)));
            }
            FieldDriverData = LDs.ToArray();
        }

        /// <summary>
        /// 53 bytes long.
        /// </summary>
        public class FinalClassificationData
        {
            public byte Position { get; set; }
            public byte NumLaps { get; set; }
            public byte GridPosition { get; set; }
            public byte Points { get; set; }
            public byte NumPitStops { get; set; }
            public ResultStatus ResultStatus { get; set; }

            public uint BestLapTimeInMS { get; set; }
            public double TotalRaceTimeInSec { get; set; }
            public byte PenaltiesTime { get; set; }
            public byte NumPenalties { get; set; }
            public byte NumTyreStins { get; set; }


            public static FinalClassificationData Create(byte[] bytes)
            {
                FinalClassificationData ReturnInstance = new FinalClassificationData();
                ByteArrayManager BAM = new ByteArrayManager(bytes);

                ReturnInstance.Position = BAM.NextByte();
                ReturnInstance.NumLaps = BAM.NextByte();
                ReturnInstance.GridPosition = BAM.NextByte();
                ReturnInstance.Points = BAM.NextByte();
                ReturnInstance.NumPitStops = BAM.NextByte();

                byte nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.ResultStatus = ResultStatus.Invalid;
                }
                else if (nb == 1)
                {
                    ReturnInstance.ResultStatus = ResultStatus.Inactive;
                }
                else if (nb == 2)
                {
                    ReturnInstance.ResultStatus = ResultStatus.Active;
                }
                else if (nb == 3)
                {
                    ReturnInstance.ResultStatus = ResultStatus.Finished;
                }
                else if (nb == 4)
                {
                    ReturnInstance.ResultStatus = ResultStatus.DNF;
                }
                else if (nb == 5)
                {
                    ReturnInstance.ResultStatus = ResultStatus.DSQ;
                }
                else if (nb == 6)
                {
                    ReturnInstance.ResultStatus = ResultStatus.NotClassified;
                }
                else if (nb == 7)
                {
                    ReturnInstance.ResultStatus = ResultStatus.Retired;
                }

                ReturnInstance.BestLapTimeInMS = BitConverter.ToUInt32(BAM.NextBytes(4),0);
                ReturnInstance.TotalRaceTimeInSec = BitConverter.ToDouble(BAM.NextBytes(8), 0);
                ReturnInstance.PenaltiesTime = BAM.NextByte();
                ReturnInstance.NumPenalties = BAM.NextByte();
                ReturnInstance.NumTyreStins = BAM.NextByte();

                return ReturnInstance;
            }

        }

        public enum ResultStatus
        {
            Invalid,
            Inactive,
            Active,
            Finished,
            DNF,
            DSQ,
            NotClassified,
            Retired
        }


    }

}