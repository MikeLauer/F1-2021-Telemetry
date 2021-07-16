using System;
using System.Collections.Generic;
using System.Text;
using TimHanewich.Toolkit;

namespace Codemasters.F1_2021
{
    public class ParticipantPacket : Packet
    {

        public byte NumberOfActiveCars { get; set; }
        public ParticipantData[] FieldParticipantData { get; set; }

        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager BAM = new ByteArrayManager(bytes);
            base.LoadBytes(BAM.NextBytes(24));

            NumberOfActiveCars = BAM.NextByte();

            List<ParticipantData> PDs = new List<ParticipantData>();
            int t = 1;
            for (t = 1; t <= 22; t++)
            {
                PDs.Add(ParticipantData.Create(BAM.NextBytes(56)));
            }
            FieldParticipantData = PDs.ToArray();
        }

        public class ParticipantData
        {
            public bool IsAiControlled { get; set; }
            //public Driver PilotingDriver { get; set; }
            public byte DriverId { get; set; }
            public byte NetworkId { get; set; }
            public Team ManufacturingTeam { get; set; }
            public bool MyTeam { get; set; }
            public byte CarRaceNumber { get; set; }
            public byte NationalityId { get; set; }
            public string Name { get; set; }
            public bool TelemetryPrivate { get; set; }

            public static ParticipantData Create(byte[] bytes)
            {
                ParticipantData ReturnInstance = new ParticipantData();
                ByteArrayManager BAM = new ByteArrayManager(bytes);

                //Get AI controlled
                byte nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.IsAiControlled = false;
                }
                else if (nb == 1)
                {
                    ReturnInstance.IsAiControlled = true;
                }


                //Get piloting driver
                //ReturnInstance.PilotingDriver = CodemastersToolkit.GetDriverFromDriverId(BAM.NextByte());
                ReturnInstance.DriverId = BAM.NextByte();

                ReturnInstance.NetworkId = BAM.NextByte();

                //Get Team
                ReturnInstance.ManufacturingTeam = ((Team)BAM.NextByte());

                nb = BAM.NextByte();
                if(nb == 0)
                {
                    ReturnInstance.MyTeam = false;
                } else if(nb == 1)
                {
                    ReturnInstance.MyTeam = true;
                }

                //Get race number
                ReturnInstance.CarRaceNumber = BAM.NextByte();

                //Get nationallity ID
                ReturnInstance.NationalityId = BAM.NextByte();

                //Get name
                byte[] byteName = BAM.NextBytes(48);
                ReturnInstance.Name = Encoding.UTF8.GetString(byteName); // UTF-8
/*
                string FullName = "";
                int t = 1;
                for (t = 1; t <= 48; t++)
                {
                    char ThisChar = Convert.ToChar(BAM.NextByte());
                    FullName = FullName + ThisChar.ToString();
                }
                ReturnInstance.Name = FullName.Trim();
*/

                //Get telemetry private or not.
                nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.TelemetryPrivate = true;
                }
                else if (nb == 1)
                {
                    ReturnInstance.TelemetryPrivate = false;
                }


                return ReturnInstance;
            }

        }
    }

}