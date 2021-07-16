using System;
using System.Collections.Generic;
using TimHanewich.Toolkit;

namespace Codemasters.F1_2021
{
    public class LapPacket : Packet
    {

        public LapData[] FieldLapData { get; set; }

        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager BAM = new ByteArrayManager(bytes);

            //Get header
            base.LoadBytes(BAM.NextBytes(24));




            //Get the next 20 data packages
            List<LapData> LDs = new List<LapData>();
            int t = 1;
            for (t = 1; t <= 22; t++)
            {
                LDs.Add(LapData.Create(BAM.NextBytes(43)));
            }
            FieldLapData = LDs.ToArray();
        }

        /// <summary>
        /// 53 bytes long.
        /// </summary>
        public class LapData
        {
            public uint LastLapTimeInMs { get; set; }
            public uint CurrentLapTimeInMs { get; set; }
            public ushort Sector1TimeInMs { get; set; }
            public ushort Sector2TimeInMs { get; set; }
            public float LapDistance { get; set; }
            public float TotalDistance { get; set; }
            public float SafetyCarDelta { get; set; }
            public byte CarPosition { get; set; }
            public byte CurrentLapNumber { get; set; }
            public PitStatus CurrentPitStatus { get; set; }
            public byte NumberPitStops { get; set; }
            public byte Sector { get; set; }
            public bool CurrentLapInvalid { get; set; }
            public byte Penalties { get; set; }
            public byte Warnings { get; set; }
            public byte NumberUnservedDriveThroughPenalties { get; set; }
            public byte NumberUnservedStopGoPenalties { get; set; }
            public byte StartingGridPosition { get; set; }
            public DriverStatus CurrentDriverStatus { get; set; }
            public ResultStatus FinalResultStatus { get; set; }
            /*
            public bool PitLaneTimerActive { get; set; }
            public ushort PitLaneTimeInLaneInMs { get; set; }
            public ushort PitStopTimerInMs { get; set; }
            public bool PitStopShouldServcePenalty { get; set; }
            */

            public static LapData Create(byte[] bytes)
            {
                LapData ReturnInstance = new LapData();
                ByteArrayManager BAM = new ByteArrayManager(bytes);

                ReturnInstance.LastLapTimeInMs = BitConverter.ToUInt32(BAM.NextBytes(4), 0);
                ReturnInstance.CurrentLapTimeInMs = BitConverter.ToUInt32(BAM.NextBytes(4), 0);

                ReturnInstance.Sector1TimeInMs = BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.Sector2TimeInMs = BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                ReturnInstance.LapDistance = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.TotalDistance = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.SafetyCarDelta = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.CarPosition = BAM.NextByte();
                ReturnInstance.CurrentLapNumber = BAM.NextByte();

                //Get pit status
                byte nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.CurrentPitStatus = PitStatus.OnTrack;
                }
                else if (nb == 1)
                {
                    ReturnInstance.CurrentPitStatus = PitStatus.PitLane;
                }
                else if (nb == 2)
                {
                    ReturnInstance.CurrentPitStatus = PitStatus.PitArea;
                }

                ReturnInstance.NumberPitStops = BAM.NextByte();

                //Get sector
                ReturnInstance.Sector = System.Convert.ToByte(BAM.NextByte() + 1);

                //Get current lap invalid
                nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.CurrentLapInvalid = false;
                }
                else if (nb == 1)
                {
                    ReturnInstance.CurrentLapInvalid = true;
                }

                //Get penalties
                ReturnInstance.Penalties = BAM.NextByte();

                ReturnInstance.Warnings = BAM.NextByte();

                ReturnInstance.NumberUnservedDriveThroughPenalties = BAM.NextByte();
                ReturnInstance.NumberUnservedStopGoPenalties = BAM.NextByte();

                //Get grid position
                ReturnInstance.StartingGridPosition = BAM.NextByte();

                //Get driver status
                nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.CurrentDriverStatus = DriverStatus.InGarage;
                }
                else if (nb == 1)
                {
                    ReturnInstance.CurrentDriverStatus = DriverStatus.FlyingLap;
                }
                else if (nb == 2)
                {
                    ReturnInstance.CurrentDriverStatus = DriverStatus.InLap;
                }
                else if (nb == 3)
                {
                    ReturnInstance.CurrentDriverStatus = DriverStatus.OutLap;
                }
                else if (nb == 4)
                {
                    ReturnInstance.CurrentDriverStatus = DriverStatus.OnTrack;
                }


                //Get result status
                nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.Invalid;
                }
                else if (nb == 1)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.Inactive;
                }
                else if (nb == 2)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.Active;
                }
                else if (nb == 3)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.Finished;
                }
                else if (nb == 4)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.DNF;
                }
                else if (nb == 5)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.DSQ;
                }
                else if (nb == 6)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.NotClassified;
                } else if(nb == 7)
                {
                    ReturnInstance.FinalResultStatus = ResultStatus.Retired;
                }

                return ReturnInstance;
            }

        }

        public enum DriverStatus
        {
            InGarage,
            FlyingLap,
            InLap,
            OutLap,
            OnTrack
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