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
        //private bool stopped = false;
        private static TcpClientChannel instance;
        //private NetworkStream stream;
        //private TcpClient client;
        private string ip;
        private int port;
        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;
        private IClientHandler clientHandler;
        private TcpClientChannel()
        {
            //read ip and port from app config !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ip = "127.0.0.1";
            port = 8000;
        }

        public void Start()
        {
            ConnectToServer();
            this.clientHandler.Start();
        }

        public void Write(string message)
        {
            clientHandler.WriteMessage(message);
        }

        public void Stop()
        {
            //this.stopped = true;
            //this.stream.Close();
            this.clientHandler.CloseHandler();
            //this.client.Close();
        }

        private void ConnectToServer () {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            //this.stream = client.GetStream();
            this.clientHandler = new ClientHandler(client); // factory ??????????????????????????????
            this.clientHandler.MessageReceived += this.HandleRecivedMessage;
        }

        public static TcpClientChannel GetInstance() {
            if (instance == null)
            {
                instance = new TcpClientChannel();
            }
            return instance;
        }

        private void HandleRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            this.MessageReceived?.Invoke(this, messageArgs);
        }
    }
}
