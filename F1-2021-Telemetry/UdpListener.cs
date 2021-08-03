using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using Codemasters.F1_2021;
using System.Threading;

namespace F1_2021_Telemetry
{
    class UdpListener
    {
        //Thread Listener;
        public bool Running = false;
        FormMain Parent;

        UdpClient udpClient;

        private ParticipantPacket ParticipantPacket = null;
        private LapPacket LapPacket = null;
        private CarStatusPacket CarStatusPacket = null;
        private SessionPacket SessionPacket = null;
        private SessionHistoryPacket SessionHistoryPacket = null;

        public CarDamagePacket CarDamagePacket { get; private set; }

        public UdpListener(FormMain parent)
        {
            Parent = parent;
        }

        public void Start()
        {
            Running = true;
            this.Run();
        }
        public void Stop()
        {
            if (Running)
            {
                udpClient.Close();
            }
            Running = false;
        }

        private void Run()
        {
            Task.Run(async () =>
            {
                using(udpClient = new UdpClient(20777))
                {
                    while(Running)
                    {
                        UdpReceiveResult result = await udpClient.ReceiveAsync();
                        byte[] bytes = result.Buffer;
                        this.ProcessResult(bytes);
                    }
                }
            });
        }

        private void ProcessResult(byte[] bytes)
        {
            Packet p = new Packet();
            p.LoadBytes(bytes);

            //Console.WriteLine(p.PacketType + " " + p.FrameIdentifier);

            if (p.PacketType == PacketType.Session) // 2/s
            {
                SessionPacket = new SessionPacket();
                SessionPacket.LoadBytes(bytes);
                this.Parent.SessionPacket = SessionPacket;
            }
            else if (p.PacketType == PacketType.Lap) // Rate
            {
                LapPacket = new LapPacket();
                LapPacket.LoadBytes(bytes);
                this.Parent.LapPacket = LapPacket;
            }
            else if (p.PacketType == PacketType.Participants) // 2/s
            {
                ParticipantPacket = new ParticipantPacket();
                ParticipantPacket.LoadBytes(bytes);
                this.Parent.ParticipantPacket = ParticipantPacket;
            }
            else if (p.PacketType == PacketType.CarStatus) // Rate
            {
                CarStatusPacket = new CarStatusPacket();
                CarStatusPacket.LoadBytes(bytes);
                this.Parent.CarStatusPacket = CarStatusPacket;
                this.Parent.UpdateData();
            } else if(p.PacketType == PacketType.CarDamage)
            {
                CarDamagePacket = new CarDamagePacket();
                CarDamagePacket.LoadBytes(bytes);
                this.Parent.CarDamagePacket = CarDamagePacket;
            }
            else if(p.PacketType == PacketType.SessionHistory) // 1/s (one driver)
            {
                SessionHistoryPacket = new SessionHistoryPacket();
                SessionHistoryPacket.LoadBytes(bytes);
                Parent.UpdateHistoryPacket(SessionHistoryPacket);
            }
        }
    }
}
