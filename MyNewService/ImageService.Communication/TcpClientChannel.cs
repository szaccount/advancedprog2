using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ImageService.Infrustracture.ToFile;

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
            port = 8080;
            this.Start();
        }

        private void Start()
        {
            //LoggerToFile.Logm("In TcpClientChannel starting client channel");
            ConnectToServer();
            //LoggerToFile.Logm("In TcpClientChannel starting client handler");
            //LoggerToFile.Logm("In TcpClientChannel finished client channel constructor");
        }

        public void Write(string message)
        {
            LoggerToFile.Logm("In TcpClientChannel writing client channel");
            clientHandler.WriteMessage(message);
            LoggerToFile.Logm("In TcpClientChannel writed client channel");
        }

        public void Stop()
        {
            //this.stopped = true;
            //this.stream.Close();
            this.clientHandler.CloseHandler();
            //this.client.Close();
        }

        private void ConnectToServer () {
            try
            {
                //LoggerToFile.Logm("connecting to server in client channel");
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
                TcpClient client = new TcpClient();
                client.Connect(ep);
                //LoggerToFile.Logm("connected to server mid!!! in client channel");
                //this.stream = client.GetStream();
                this.clientHandler = new ClientHandler(client); // factory ??????????????????????????????
                //LoggerToFile.Logm("starting client handler");
                this.clientHandler.Start();
                this.clientHandler.MessageReceived += this.HandleRecivedMessage;
                //LoggerToFile.Logm("connected to server in client channel");
            }
            catch (Exception exc)
            {
                //put here the code to make the background grey and never contuct the server again !!!!!!!!! also in logs !!!!!!!!!!!!!!!!!!!
            }
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
            //LoggerToFile.Logm("In TcpClientChannel received message to handle: " + messageArgs.Message);
            this.MessageReceived?.Invoke(this, messageArgs);
        }
    }
}
