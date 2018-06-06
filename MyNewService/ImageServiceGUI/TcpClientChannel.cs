using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ImageService.Communication;
using System.Configuration;

namespace ImageServiceGUI
{
    /// <summary>
    /// TcpClientChannel singelton, single communication channel of clients of same group with server
    /// </summary>
    public class TcpClientChannel
    {
        private static TcpClientChannel instance;
        private string ip;
        private int port;
        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;
        private IClientHandler clientHandler;

        public bool IsConnected { get; set; }

        // <summary>
        /// private cto'r connecting to ip and port from config
        /// </summary>
        private TcpClientChannel()
        {

            //reading from the configuration file
            var appSettings = ConfigurationManager.AppSettings;
            //after ?? are the default values if there is no reference for them in the app config file
            string tmpIp = appSettings["Ip"] ?? "127.0.0.1";
            int tmpPort;
            //if conversion failed, put default value
            if (!Int32.TryParse(appSettings["Port"], out tmpPort))
                tmpPort = 8080;

            this.ip = tmpIp;
            this.port = tmpPort;
            this.Start();
        }

        // <summary>
        /// connecting to the server, starting the channel
        /// </summary>
        private void Start()
        {
            ConnectToServer();
        }

        /// <summary>
        /// write message to the channel
        /// </summary>
        public void Write(string message)
        {
            if (IsConnected)
                clientHandler.WriteMessage(message);
        }

        /// <summary>
        /// stop the channel of communication
        /// </summary>
        public void Stop()
        {
            this.clientHandler.CloseHandler();
        }

        /// <summary>
        /// connection to the server method
        /// </summary>
        private void ConnectToServer () {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.ip), this.port);
                TcpClient client = new TcpClient();
                client.Connect(ep);
                this.clientHandler = new ClientHandler(client);
                this.clientHandler.Start();
                this.clientHandler.MessageReceived += this.HandleRecivedMessage;
                this.IsConnected = client.Connected;
            }
            catch (Exception exc)
            {
                this.IsConnected = false;
            }
        }

        /// <summary>
        /// method for implemeting the singleton type of the class
        /// </summary>
        /// <returns> the instance of the class </returns>
        public static TcpClientChannel GetInstance() {
            if (instance == null)
            {
                instance = new TcpClientChannel();
            }
            return instance;
        }

        /// <summary>
        /// invoke the message that was received, pass higher
        /// </summary>
        /// <param name="sender">the sender of the message</param>
        /// <param name="messageArgs">the message that was received</param>
        private void HandleRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            this.MessageReceived?.Invoke(this, messageArgs);
        }
    }
}
