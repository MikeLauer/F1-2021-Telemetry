using System;
using System.Collections.Generic;
using System.Drawing;

namespace Codemasters.F1_2021
{
    public static class CodemastersToolkit
    {
        public static PacketType GetPacketType(byte[] bytes)
        {
            if (bytes.Length == 1464)
            {
                return PacketType.Motion;
            }
            else if (bytes.Length == 625)
            {
                return PacketType.Session;
            }
            else if (bytes.Length == 970)
            {
                return PacketType.Lap;
            }
            else if (bytes.Length == 36)
            {
                return PacketType.Event;
            }
            else if (bytes.Length == 1257)
            {
                return PacketType.Participants;
            }
            else if (bytes.Length == 1102)
            {
                return PacketType.CarSetup;
            }
            else if (bytes.Length == 1347)
            {
                return PacketType.CarTelemetry;
            }
            else if (bytes.Length == 1058)
            {
                return PacketType.CarStatus;
            }
            else if (bytes.Length == 839)
            {
                return PacketType.FinalClassification;
            }
            else if (bytes.Length == 1191)
            {
                return PacketType.LobbyInfo;
            }
            else if (bytes.Length == 882)
            {
                return PacketType.CarDamage;
            }
            else if (bytes.Length == 1155)
            {
                return PacketType.SessionHistory;
            }
            else
            {
                throw new Exception("Packet type not recognized.");
            }
        }

        public static Track GetTrackFromTrackId(byte id)
        {
            Track ReturnTrack = Track.Melbourne;

            if (id == 0)
            {
                ReturnTrack = Track.Melbourne;
            }
            else if (id == 1)
            {
                ReturnTrack = Track.PaulRicard;
            }
            else if (id == 2)
            {
                ReturnTrack = Track.Shanghai;
            }
            else if (id == 3)
            {
                ReturnTrack = Track.Bahrain;
            }
            else if (id == 4)
            {
                ReturnTrack = Track.Catalunya;
            }
            else if (id == 5)
            {
                ReturnTrack = Track.Monaco;
            }
            else if (id == 6)
            {
                ReturnTrack = Track.Montreal;
            }
            else if (id == 7)
            {
                ReturnTrack = Track.Silverstone;
            }
            else if (id == 8)
            {
                ReturnTrack = Track.Hockenheim;
            }
            else if (id == 9)
            {
                ReturnTrack = Track.Hungaroring;
            }
            else if (id == 10)
            {
                ReturnTrack = Track.Spa;
            }
            else if (id == 11)
            {
                ReturnTrack = Track.Monza;
            }
            else if (id == 12)
            {
                ReturnTrack = Track.Singapore;
            }
            else if (id == 13)
            {
                ReturnTrack = Track.Suzuka;
            }
            else if (id == 14)
            {
                ReturnTrack = Track.AbuDhabi;
            }
            else if (id == 15)
            {
                ReturnTrack = Track.Texas;
            }
            else if (id == 16)
            {
                ReturnTrack = Track.Brazil;
            }
            else if (id == 17)
            {
                ReturnTrack = Track.Austria;
            }
            else if (id == 18)
            {
                ReturnTrack = Track.Sochi;
            }
            else if (id == 19)
            {
                ReturnTrack = Track.Mexico;
            }
            else if (id == 20)
            {
                ReturnTrack = Track.Baku;
            }
            else if (id == 21)
            {
                ReturnTrack = Track.SakhirShort;
            }
            else if (id == 22)
            {
                ReturnTrack = Track.SilverstoneShort;
            }
            else if (id == 23)
            {
                ReturnTrack = Track.TexasShort;
            }
            else if (id == 24)
            {
                ReturnTrack = Track.SuzukaShort;
            }
            else if (id == 25)
            {
                ReturnTrack = Track.Hanoi;
            }
            else if (id == 26)
            {
                ReturnTrack = Track.Zandvoort;
            }
            else if (id == 27)
            {
                ReturnTrack = Track.Imola;
            }
            else if (id == 28)
            {
                ReturnTrack = Track.Pertimao;
            }
            else if (id == 29)
            {
                ReturnTrack = Track.Jeddah;
            }

            return ReturnTrack;
        }
        public static string GetTrackFriendlyName(Track t)
        {
            string ToReturn = t.ToString();

            if (t == Track.Melbourne)
            {
                ToReturn = "Melbourne (Australia)";
            }
            else if (t == Track.PaulRicard)
            {
                ToReturn = "Paul Ricard (France)";
            }
            else if (t == Track.Shanghai)
            {
                ToReturn = "Shanghai (China)";
            }
            else if (t == Track.Bahrain)
            {
                ToReturn = "Sakhir (Bahrain)";
            }
            else if (t == Track.Catalunya)
            {
                ToReturn = "Catalunya (Spain)";
            }
            else if (t == Track.Monaco)
            {
                ToReturn = "Monaco";
            }
            else if (t == Track.Montreal)
            {
                ToReturn = "Montreal (Canada)";
            }
            else if (t == Track.Silverstone)
            {
                ToReturn = "Silverstone (GB)";
            }
            else if (t == Track.Hockenheim)
            {
                ToReturn = "Hockenheim (Germany)";
            }
            else if (t == Track.Hungaroring)
            {
                ToReturn = "Hungaroring";
            }
            else if (t == Track.Spa)
            {
                ToReturn = "Spa (Belgium)";
            }
            else if (t == Track.Monza)
            {
                ToReturn = "Monza (Italy)";
            }
            else if (t == Track.Singapore)
            {
                ToReturn = "Singapore";
            }
            else if (t == Track.Suzuka)
            {
                ToReturn = "Suzuka (Japan)";
            }
            else if (t == Track.AbuDhabi)
            {
                ToReturn = "Abu Dhabi";
            }
            else if (t == Track.Texas)
            {
                ToReturn = "Texas (US)";
            }
            else if (t == Track.Brazil)
            {
                ToReturn = "Brazil";
            }
            else if (t == Track.Austria)
            {
                ToReturn = "Spielberg (Austria)";
            }
            else if (t == Track.Sochi)
            {
                ToReturn = "Sochi (Russia)";
            }
            else if (t == Track.Mexico)
            {
                ToReturn = "Mexico";
            }
            else if (t == Track.Baku)
            {
                ToReturn = "Baku (Azerbaijan)";
            }
            else if (t == Track.SakhirShort)
            {
                ToReturn = "Bahrain (Short)";
            }
            else if (t == Track.SilverstoneShort)
            {
                ToReturn = "Silverstone (Short)";
            }
            else if (t == Track.TexasShort)
            {
                ToReturn = "US (Short)";
            }
            else if (t == Track.SuzukaShort)
            {
                ToReturn = "Japan (Short)";
            }
            else if (t == Track.Hanoi)
            {
                ToReturn = "Hanoi (Vietnam)";
            }
            else if (t == Track.Zandvoort)
            {
                ToReturn = "Zandvoort (Netherlands)";
            }
            else if (t == Track.Imola)
            {
                ToReturn = "Italien (Imola)";
            }
            else if (t == Track.Pertimao)
            {
                ToReturn = "Pertimao (Portugal)";
            }
            else if (t == Track.Jeddah)
            {
                ToReturn = "Jeddah (Saudi Arabia)";
            }

            return ToReturn;
        }

        /// <summary>
        /// This returns the name that should be displayed in a leaderboard (i.e. "L. Hamilton")
        /// </summary>
        public static string GetDriverDisplayNameFromDriver(Driver d)
        {
            string s = "Unknown";

            if (d == Driver.LewisHamilton)
            {
                s = "L. Hamilton";
            }
            else if (d == Driver.ValtteriBottas)
            {
                s = "V. Bottas";
            }
            else if (d == Driver.RomainGrosjean)
            {
                s = "R. Grosjean";
            }
            else if (d == Driver.KevinMagnussen)
            {
                s = "K. Magnussen";
            }
            else if (d == Driver.ValtteriBottas)
            {
                s = "V. Bottas";
            }
            else if (d == Driver.CarlosSainz)
            {
                s = "C. Sainz";
            }
            else if (d == Driver.LandoNorris)
            {
                s = "L. Norris";
            }
            else if (d == Driver.KimiRaikkonen)
            {
                s = "K. Raikkonen";
            }
            else if (d == Driver.AntonioGiovinazzi)
            {
                s = "A. Giovinazzi";
            }
            else if (d == Driver.MaxVerstappen)
            {
                s = "M. Verstappen";
            }
            else if (d == Driver.AlexanderAlbon)
            {
                s = "A. Albon";
            }
            else if (d == Driver.DanielRicciardo)
            {
                s = "D. Ricciardo";
            }
            else if (d == Driver.NicoHulkenburg)
            {
                s = "N. Hulkenburg";
            }
            else if (d == Driver.SebastianVettel)
            {
                s = "S. Vettel";
            }
            else if (d == Driver.CharlesLeclerc)
            {
                s = "C. Leclerc";
            }
            else if (d == Driver.PierreGasly)
            {
                s = "P. Gasly";
            }
            else if (d == Driver.DaniilKvyat)
            {
                s = "D. Kvyat";
            }
            else if (d == Driver.GeorgeRussell)
            {
                s = "G. Russell";
            }
            else if (d == Driver.NicholasLatifi)
            {
                s = "N. Latifi";
            }
            else if (d == Driver.RobertKubica)
            {
                s = "R. Kubica";
            }
            else if (d == Driver.SergioPerez)
            {
                s = "S. Perez";
            }
            else if (d == Driver.LanceStroll)
            {
                s = "L. Stroll";
            }
            else if (d == Driver.EstebanOcon)
            {
                s = "E. Ocon";
            }
            else if (d == Driver.NicholasLatifi)
            {
                s = "N. Latifi";
            }

            return s;
        }

        #region "3 Driver display letters"

        public static string GetDriverThreeLetters(Driver d)
        {
            List<KeyValuePair<Driver, string>> KVPs = new List<KeyValuePair<Driver, string>>();
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.MaxVerstappen, "VER"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.ValtteriBottas, "BOT"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.LewisHamilton, "HAM"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.LandoNorris, "NOR"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.AlexanderAlbon, "ALB"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.CarlosSainz, "SAI"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.DaniilKvyat, "KVY"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.LanceStroll, "STR"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.EstebanOcon, "OCO"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.PierreGasly, "GAS"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.DanielRicciardo, "RIC"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.SebastianVettel, "VET"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.CharlesLeclerc, "LEC"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.KimiRaikkonen, "RAI"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.AntonioGiovinazzi, "GIO"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.GeorgeRussell, "RUS"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.KevinMagnussen, "MAG"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.RomainGrosjean, "GRO"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.NicholasLatifi, "LAT"));
            KVPs.Add(new KeyValuePair<Driver, string>(Driver.SergioPerez, "PER"));

            foreach (KeyValuePair<Driver, string> KVP in KVPs)
            {
                if (KVP.Key == d)
                {
                    return KVP.Value;
                }
            }

            //If we've gotten this far, we dont have it
            string displayname = d.ToString();
            return displayname.Substring(0, 3).ToUpper();
        }

        #endregion
    }

}