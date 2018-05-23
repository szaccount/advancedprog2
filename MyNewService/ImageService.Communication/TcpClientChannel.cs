using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ImageService.Communication
{
    public class TcpClientChannel
    {
        private bool stopped = false;
        private static TcpClientChannel instance;
        private NetworkStream stream;
        private TcpClient client;
        private string ip;
        private int port;
        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;
        private IClientHandler handler;
        private TcpClientChannel() {}

        public void Start()
        {
            connectToServer();
            Task task = new Task(() => {
                while (!stopped)
                {
                    try
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            MessageCommunicationEventArgs messageReceived = new MessageCommunicationEventArgs {
                                Message = reader.ReadString()
                            };
                            this.MessageReceived?.Invoke(this, messageReceived);
                        }
                    }
                    catch (Exception)
                    {   
                        Stop();
                    }
                }
            });
        }

        public void Write(string message)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                try
                {
                    writer.Write(message);
                }
                catch (Exception)
                {
                    Stop();
                } 
            }
        }

        public void Stop()
        {
            this.stopped = true;
            this.stream.Close();
            this.client.Close();
        }

        public void connectToServer () {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            this.client = new TcpClient();
            client.Connect(ep);
            this.stream = client.GetStream();
        }

        public static TcpClientChannel GetInstacnce() {
            if (instance == null)
            {
                instance = new TcpClientChannel();
            }
            return instance;
        }
    }
}
