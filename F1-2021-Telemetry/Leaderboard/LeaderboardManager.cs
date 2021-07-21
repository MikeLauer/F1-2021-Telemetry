using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codemasters.F1_2021;

namespace F1_2021_Telemetry
{
    /// <summary>
    /// Creates a leaderboard based on received packets
    /// </summary>
    class LeaderboardManager
    {
        private LapPacket LapPacket;
        private ParticipantPacket ParticipantPacket;
        private CarStatusPacket CarStatusPacket;
        private LapPacket LastLapPacket;
        private SessionPacket SessionPacket;
        private SessionHistoryPacket[] SessionHistoryPackets;

        long[,] DriverTimestampOfDistances = null; // 20 drivers, Laps * TrackLength / 1/Accuracy
        int NumberOfMeasurepoints = -1;
        readonly float MeasurepointAccuracy = 0.05f; // %

        public bool LiveTiming = false; // Enable/disable live timing

        private int NumberOfDrivers;


        public void InitSessionInfo(int numberOfDrivers, int numberOfLaps, float trackLength)
        {
            NumberOfDrivers = numberOfDrivers;

            this.NumberOfMeasurepoints = (int) Math.Floor( (numberOfLaps * trackLength) / (1 / MeasurepointAccuracy) );
            this.DriverTimestampOfDistances = new long[NumberOfDrivers, this.NumberOfMeasurepoints];

            this.Clear();
        }

        public void Clear()
        {
            Console.WriteLine("Clearing data");
            SessionHistoryPackets = new SessionHistoryPacket[NumberOfDrivers];
            for (short i = 0; i < this.NumberOfDrivers; i++)
            {
                SessionHistoryPackets[i] = null;

                if(this.NumberOfMeasurepoints > -1)
                {
                    for(int k = 0; k < this.NumberOfMeasurepoints; k++)
                    {
                        this.DriverTimestampOfDistances[i, k] = -1;
                    }
                }                
            }
            LastLapPacket = null;
        }

        /// <summary>
        /// Process incoming packets
        /// </summary>
        /// <param name="participantPacket"></param>
        /// <param name="lapPacket"></param>
        /// <param name="carStatusPacket"></param>
        /// <param name="sessionPacket"></param>
        public void UpdateData(ParticipantPacket participantPacket, LapPacket lapPacket, CarStatusPacket carStatusPacket, SessionPacket sessionPacket)
        {
            this.LastLapPacket = this.LapPacket;
            this.LapPacket = lapPacket;
            this.ParticipantPacket = participantPacket;
            this.SessionPacket = sessionPacket;
            this.CarStatusPacket = carStatusPacket;           

            for (int i = 0; i < NumberOfDrivers; i++) // for each driver
            {
                if (lapPacket.FieldLapData[i].LapDistance < 0) continue;

                if (sessionPacket.SessionTypeMode == SessionPacket.SessionType.Race) // If session type is 'Race' compute distances
                    this.SetTimestampForGapCalculation(sessionPacket, lapPacket, i); // Gaps
            }
        }

        public void UpdateHistoryPacket(SessionHistoryPacket sessionHistoryPacket)
        {
            if (this.SessionHistoryPackets != null && sessionHistoryPacket.CarIndex < this.SessionHistoryPackets.Length) // F1 2020/21 have 22 drivers. I consider only 20 right now
                this.SessionHistoryPackets[sessionHistoryPacket.CarIndex] = sessionHistoryPacket;
        }

        /// <summary>
        /// Compute the gaps between the drivers
        /// </summary>
        /// <param name="sessionPacket"></param>
        /// <param name="lapPacket"></param>
        /// <param name="index"></param>
        private void SetTimestampForGapCalculation(SessionPacket sessionPacket, LapPacket lapPacket, int index)
        {
            if (lapPacket.FieldLapData[index].TotalDistance >= 0)
            {
                int measurepoint = this.GetMeasurepointFromDistance(lapPacket.FieldLapData[index].TotalDistance, sessionPacket.TrackLengthMeters);
                if (measurepoint > this.NumberOfMeasurepoints - 1)
                {
                    Console.WriteLine("Exceeding max number of measurepoints " + index);
                    measurepoint = this.NumberOfMeasurepoints - 1;
                }

                if (this.DriverTimestampOfDistances[index, measurepoint] == -1)
                {
                    long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    this.DriverTimestampOfDistances[index, measurepoint] = timestamp; //sessionPacket.SessionTime;
                }
            }
        }

        private int GetMeasurepointFromDistance(float drivenDistance, float trackLength)
        {
            return (int)Math.Floor(drivenDistance / (trackLength * MeasurepointAccuracy));
        }

        /// <summary>
        /// Generates and returns the leaderboard. 
        /// 
        /// WARNING: This method is a liiiittle long, but it's basically just setting values.
        /// </summary>
        /// <returns>leaderboard</returns>
        public Leaderboard getLeaderboard()
        {
            Leaderboard leaderboard = new Leaderboard(); // create empty leaderboard

            for (byte i = 0; i < 20; i++) // for each driver
            {
                LeaderboardDriver driver = new LeaderboardDriver(); // create empty driver
                driver.Index = i; // Set driver index (index from game order, not position order)

                driver.SetTeamColor(((int)this.ParticipantPacket.FieldParticipantData[i].ManufacturingTeam)); // Set team color

                driver.CarPosition = this.LapPacket.FieldLapData[i].CarPosition; // Set car position

                driver.DriverName = this.ParticipantPacket.FieldParticipantData[i].Name + " " + this.ParticipantPacket.FieldParticipantData[i].NetworkId; // Set driver name

                driver.CurrentLapNumber = this.LapPacket.FieldLapData[i].CurrentLapNumber.ToString(); // Current lap number                

                driver.SetBestAndLastLapTime(0, this.LapPacket.FieldLapData[i].LastLapTimeInMs);
                if (this.SessionHistoryPackets[i] != null) // History is known
                {
                    byte bestLapNumber = this.SessionHistoryPackets[i].BestLapTimeLapNumber;
                    if (bestLapNumber != 0) // TODO: off by 1? What is default?
                    {
                        uint bestLapTime = this.SessionHistoryPackets[i].LapsHistoryData[bestLapNumber - 1].LapTimeInMs;
                        driver.SetBestAndLastLapTime(bestLapTime, this.LapPacket.FieldLapData[i].LastLapTimeInMs);
                    }
                }

                byte tyre = this.CarStatusPacket.FieldCarStatusData[i].EquippedVisualTyreCompoundId; // Visual compound (there are more actual compounds)
                switch (tyre) // Set tyre
                {
                    case 16:
                        driver.Tyre = "Soft";
                        break;
                    case 17:
                        driver.Tyre = "Med";
                        break;
                    case 18:
                        driver.Tyre = "Hard";
                        break;
                    case 7:
                        driver.Tyre = "Inter";
                        break;
                    case 8:
                        driver.Tyre = "Wet";
                        break;
                }
                driver.Tyre += " (" + this.CarStatusPacket.FieldCarStatusData[i].TyreAgeLaps + ")";

                // Pit Count / Status
                PitStatus pitStatus = this.LapPacket.FieldLapData[i].CurrentPitStatus;
                if (pitStatus == PitStatus.PitLane || pitStatus == PitStatus.PitArea || this.LapPacket.FieldLapData[i].CurrentDriverStatus == LapPacket.DriverStatus.InGarage) // If somewhere in the pit
                {
                    driver.PitStatus = "In Pit";
                }
                else
                {
                    driver.PitStatus = this.LapPacket.FieldLapData[i].NumberPitStops.ToString(); // Otherwise set pit status to pitstop count
                }

                LapPacket.ResultStatus status = this.LapPacket.FieldLapData[i].FinalResultStatus;                
                if (status == LapPacket.ResultStatus.DSQ || status == LapPacket.ResultStatus.DNF || status == LapPacket.ResultStatus.Finished || status == LapPacket.ResultStatus.Retired) // If out of the race
                {
                    driver.PitStatus = status.ToString();
                }

                // Position difference to start grid
                if (this.SessionPacket.SessionTypeMode == SessionPacket.SessionType.Race || this.SessionPacket.SessionTypeMode == SessionPacket.SessionType.Race2)
                {
                    int positionDifference = this.LapPacket.FieldLapData[i].StartingGridPosition - this.LapPacket.FieldLapData[i].CarPosition; // Position difference
                    String positionDifferenceString = positionDifference.ToString();
                    if (positionDifference > 0)
                    {
                        positionDifferenceString = "+" + positionDifferenceString; // Add a + for asthetics
                    }
                    driver.PositionDifferenceToStartgrid = positionDifferenceString; // Set position difference
                }
                else
                {
                    driver.PositionDifferenceToStartgrid = "";
                }

                driver.DriverStatus = this.LapPacket.FieldLapData[i].CurrentDriverStatus.ToString(); // Set driver status (OutLap, InLap, etc...)

                this.SetLastSectorTimes(driver, i);

                this.SetBestSectorTimes(driver, i);

                driver.HasOvertaken = false; // Set has ovetaken to false
                if (LastLapPacket != null && this.LastLapPacket.FieldLapData[i].CarPosition > this.LapPacket.FieldLapData[i].CarPosition) // If driver has overtaken someone
                {
                    driver.HasOvertaken = true; // Set has overtaken to true
                    driver.OvertakenPosition = LastLapPacket.FieldLapData[i].CarPosition; // Set overtaken position to previous position
                }

                driver.LapDistance = this.LapPacket.FieldLapData[i].LapDistance; // Set lap distance of current lap

                driver.DrsAllowed = this.CarStatusPacket.FieldCarStatusData[i].DrsAllowed;

                driver.TimePenaltiesInSeconds = this.LapPacket.FieldLapData[i].Penalties;

                leaderboard.SetDriverToPosition(driver.CarPosition, driver); // Set driver on leaderboard to right position                
            }

            leaderboard = this.ComputeOrderDependentValues(leaderboard);
            return leaderboard;
        }

        private void SetLastSectorTimes(LeaderboardDriver driver, int index)
        {
            SessionHistoryPacket.LapHistoryData lastLap = null;
            if (this.SessionHistoryPackets[index] != null)
            {
                for (int k = 99; k >= 0; k--)
                {
                    if (this.SessionHistoryPackets[index].LapsHistoryData[k].LapTimeInMs != 0)
                    {
                        lastLap = this.SessionHistoryPackets[index].LapsHistoryData[k];
                        break;
                    }
                }
            }

            if (this.LapPacket.FieldLapData[index].CurrentDriverStatus == LapPacket.DriverStatus.InGarage)
            {
                if (lastLap != null)
                {
                    driver.SetSector1LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector2LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector3LastColor(LeaderboardFlags.Yellow);

                    driver.Sector1Last = lastLap.Sector1TimeInMs;
                    driver.Sector2Last = lastLap.Sector2TimeInMs;
                    driver.Sector3Last = lastLap.Sector3TimeInMs;
                }
            }
            else
            {
                if (this.LapPacket.FieldLapData[index].Sector == 1) // In sector 1
                {
                    driver.SetSector1LastColor(LeaderboardFlags.White);
                    driver.SetSector2LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector3LastColor(LeaderboardFlags.Yellow);

                    if (LiveTiming)
                        driver.Sector1Last = this.LapPacket.FieldLapData[index].CurrentLapTimeInMs;

                    if (lastLap != null)
                        driver.Sector2Last = lastLap.Sector2TimeInMs;

                    if (lastLap != null)
                        driver.Sector3Last = lastLap.Sector3TimeInMs;
                }
                else if (this.LapPacket.FieldLapData[index].Sector == 2) // In sector 2
                {
                    driver.SetSector1LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector2LastColor(LeaderboardFlags.White);
                    driver.SetSector3LastColor(LeaderboardFlags.Yellow);

                    driver.Sector1Last = this.LapPacket.FieldLapData[index].Sector1TimeInMs;

                    if (LiveTiming)
                        driver.Sector2Last = this.LapPacket.FieldLapData[index].CurrentLapTimeInMs - this.LapPacket.FieldLapData[index].Sector1TimeInMs;

                    if (lastLap != null)
                        driver.Sector3Last = lastLap.Sector3TimeInMs;
                }
                else if (this.LapPacket.FieldLapData[index].Sector == 3) // In sector 3
                {
                    driver.SetSector1LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector2LastColor(LeaderboardFlags.Yellow);
                    driver.SetSector3LastColor(LeaderboardFlags.White);

                    driver.Sector1Last = this.LapPacket.FieldLapData[index].Sector1TimeInMs;

                    driver.Sector2Last = this.LapPacket.FieldLapData[index].Sector2TimeInMs;

                    if (LiveTiming)
                        driver.Sector3Last = this.LapPacket.FieldLapData[index].CurrentLapTimeInMs - this.LapPacket.FieldLapData[index].Sector1TimeInMs - this.LapPacket.FieldLapData[index].Sector2TimeInMs;
                }

            }
        }

        private void SetBestSectorTimes(LeaderboardDriver driver, int index)
        {
            if (this.SessionHistoryPackets[index] != null)
            {
                int bestSectorLapNum = this.SessionHistoryPackets[index].BestSector1LapNumbers;
                if(bestSectorLapNum > 0)
                    driver.Sector1Best = this.SessionHistoryPackets[index].LapsHistoryData[bestSectorLapNum-1].Sector1TimeInMs;

                bestSectorLapNum = this.SessionHistoryPackets[index].BestSector2LapNumbers;
                if (bestSectorLapNum > 0)
                    driver.Sector2Best = this.SessionHistoryPackets[index].LapsHistoryData[bestSectorLapNum-1].Sector2TimeInMs;

                bestSectorLapNum = this.SessionHistoryPackets[index].BestSector3LapNumbers;
                if (bestSectorLapNum > 0)
                    driver.Sector3Best = this.SessionHistoryPackets[index].LapsHistoryData[bestSectorLapNum-1].Sector3TimeInMs;

                if(driver.Sector1Best == driver.Sector1Last)
                    driver.SetSector1LastColor(LeaderboardFlags.Green);

                if (driver.Sector2Best == driver.Sector2Last)
                    driver.SetSector2LastColor(LeaderboardFlags.Green);

                if (driver.Sector3Best == driver.Sector3Last)
                    driver.SetSector3LastColor(LeaderboardFlags.Green);
            }
            driver.SetDefaultBestSectorColours();
        }

        /// <summary>
        /// Computes the best values, i.e. best lap, best sector 1, etc.
        /// </summary>
        /// <param name="leaderboard"></param>
        /// <returns></returns>
        private Leaderboard ComputeOrderDependentValues(Leaderboard leaderboard)
        {
            // To find the best lap and best sectors
            byte BestLapTimeIndex = 255;
            uint BestLapTime = int.MaxValue;
            byte BestSector1Index = 255;
            uint BestSector1 = int.MaxValue;
            byte BestSector2Index = 255;
            uint BestSector2 = int.MaxValue;
            byte BestSector3Index = 255;
            uint BestSector3 = int.MaxValue;

            for (byte i = 0; i < 20; i++) // for each driver (order is race position)
            {
                LeaderboardDriver driver = leaderboard.getDriver(i); // get driver from leaderboard
                if (driver == null) continue; // If no driver available, e.g. only 19 drivers

                LeaderboardDriver driverInFront = (i > 0) ? leaderboard.getDriver(i - 1) : null;

                uint localBestLapTime = driver.BestLapTime;
                uint localLastLapTime = driver.LastLapTime;

                if (driverInFront != null)
                {

                    // Time delta to next drivers best lap
                    if (localBestLapTime > 0 && driverInFront.BestLapTime > 0)
                    {
                        int delta = (int)(localBestLapTime - driverInFront.BestLapTime);
                        driver.DeltaBestLapInterval = delta; // Set delta of best lap                    
                    }

                    // Time delta to next drivers last lap
                    if (localLastLapTime > 0 && driverInFront.LastLapTime > 0)
                    {
                        int delta = (int)(localLastLapTime - driverInFront.LastLapTime);
                        driver.DeltaLastLapInterval = delta; // Set delta of last lap
                    }

                    // Gaps   
                    if (this.SessionPacket.SessionTypeMode == SessionPacket.SessionType.Race && this.LapPacket.FieldLapData[driver.Index].TotalDistance >= 0) // Only in race session, compute distances on the track
                    {

                        if (i > 0) // Not for the first driver
                        {
                            int measurepoint = this.GetMeasurepointFromDistance(this.LapPacket.FieldLapData[driver.Index].TotalDistance, SessionPacket.TrackLengthMeters);
                            if (measurepoint > this.NumberOfMeasurepoints - 1 || measurepoint < 0)
                                measurepoint = 0;

                            if (this.DriverTimestampOfDistances[driver.Index, measurepoint] != -1)
                            {
                                uint gap;

                                // To driver in front
                                gap = (uint) (this.DriverTimestampOfDistances[driver.Index, measurepoint] - this.DriverTimestampOfDistances[driverInFront.Index, measurepoint]);
                                driver.GapInterval = gap;

                                // To leader
                                gap = (uint) (this.DriverTimestampOfDistances[driver.Index, measurepoint] - this.DriverTimestampOfDistances[leaderboard.getDriver(0).Index, measurepoint]);
                                driver.GapLeader = gap;
                            }
                        }
                    }
                } // End driver in front != null

                // Best Lap
                if (localBestLapTime < BestLapTime && localBestLapTime != 0) // If best lap of current driver is overall best
                {
                    BestLapTime = localBestLapTime; // Temporarily save best lap time
                    BestLapTimeIndex = i; // Temporarily save index of driver
                }

                // Best Sector 1
                uint bestSector1 = driver.Sector1Best;
                if (bestSector1 < BestSector1 && bestSector1 != 0) // If best sector 1 time of current driver is overall best
                {
                    BestSector1 = bestSector1; // Temporarily save best sector 1 time
                    BestSector1Index = i; // Temporarily save index of driver
                }

                // Best Sector 2
                uint bestSector2 = driver.Sector2Best;
                if (bestSector2 < BestSector2 && bestSector2 != 0) // If best sector 2 time of current driver is overall best
                {
                    BestSector2 = bestSector2; // Temporarily save best sector 2 time
                    BestSector2Index = i; // Temporarily save index of driver
                }

                // Best Sector 3
                uint bestSector3 = driver.Sector3Best;
                if (bestSector3 < BestSector3 && bestSector3 != 0) // If best sector 3 time of current driver is overall best
                {
                    BestSector3 = bestSector3; // Temporarily save best sector 3 time
                    BestSector3Index = i; // Temporarily save index of driver
                }

            }

            // Delta to best lap in session
            if (BestLapTimeIndex != 255) // If best lap exists
            {
                for (int i = 0; i < NumberOfDrivers; i++)
                {
                    LeaderboardDriver driver = leaderboard.getDriver(i);
                    if(driver != null && driver.BestLapTime > 0)
                        driver.DeltaBestLap = (int)(driver.BestLapTime - leaderboard.getDriver(BestLapTimeIndex).BestLapTime);
                }
            }

            if (BestLapTimeIndex != 255)
                leaderboard.DriverData[BestLapTimeIndex].SetBestLapTimeColor(LeaderboardFlags.Purple); // Set best lap time color to purple

            if (BestSector1Index != 255)
                leaderboard.DriverData[BestSector1Index].SetSector1BestColor(LeaderboardFlags.Purple); // Set best sector 1 time color to purple
            if (BestSector2Index != 255)
                leaderboard.DriverData[BestSector2Index].SetSector2BestColor(LeaderboardFlags.Purple); // Set best sector 2 time color to purple
            if (BestSector3Index != 255)
                leaderboard.DriverData[BestSector3Index].SetSector3BestColor(LeaderboardFlags.Purple); // Set best sector 3 time color to purple

            if(BestSector1Index != 255 && BestSector2Index != 255 && BestSector3Index != 255)
            {
                uint theoredicalBestLap = BestSector1 + BestSector2 + BestSector3;
                leaderboard.TheoredicalBestLap = theoredicalBestLap;
            }

            return leaderboard;
        }
    }
}
