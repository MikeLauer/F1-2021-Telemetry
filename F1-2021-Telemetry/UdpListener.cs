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

        public UdpListener(FormMain parent)
        {
            Parent = parent;
        }

        public void Start()
        {
            Running = true;
            this.Run2();
            /*ThreadStart childref = new ThreadStart(Run);
            Console.WriteLine("Creating the Listener thread");

            Listener = new Thread(childref);
            Listener.Start();*/
        }
        public void Stop()
        {
            if (Running)
            {
                udpClient.Close();
                //Listener.Abort();
            }
            Running = false;
        }

        private void Run2()
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

        private void Run()
        {
            udpClient = new UdpClient(20777);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 20777);

            try
            {               
                while (Running)
                {
                    byte[] bytes = udpClient.Receive(ref groupEP);

                    this.ProcessResult(bytes);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
            finally
            {
                udpClient.Close();
            }
            Console.WriteLine("Server off");

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
            }
            else if (p.PacketType == PacketType.Lap) // Rate
            {
                LapPacket = new LapPacket();
                LapPacket.LoadBytes(bytes);
            }
            else if (p.PacketType == PacketType.Participants) // 2/s
            {
                ParticipantPacket = new ParticipantPacket();
                ParticipantPacket.LoadBytes(bytes);
            }
            else if (p.PacketType == PacketType.CarStatus) // Rate
            {
                CarStatusPacket = new CarStatusPacket();
                CarStatusPacket.LoadBytes(bytes);
                this.sendUpdate();
            }
            else if(p.PacketType == PacketType.SessionHistory) // 1/s (one driver)
            {
                SessionHistoryPacket = new SessionHistoryPacket();
                SessionHistoryPacket.LoadBytes(bytes);
            }
        }

        private void sendUpdate()
        {
            if (ParticipantPacket != null && LapPacket != null && CarStatusPacket != null && SessionPacket != null && SessionHistoryPacket != null)
            {
                //Parent.AddPacketsToQueue(packets);
                if (Parent != null)
                    Parent.UpdateData(ParticipantPacket, LapPacket, CarStatusPacket, SessionPacket, SessionHistoryPacket);
            }
        }
    }
}
