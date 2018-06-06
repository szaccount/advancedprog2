using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;

namespace ImageService.Communication
{
    /// <summary>
    /// class implementing the IclientHandler interface
    /// </summary>
    public class ClientHandler : IClientHandler
    {

        
        private bool running;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private TcpClient client;

        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;

        public event EventHandler<IClientHandlerCloseEventArgs> ClosingClientHandler;

        /// <summary>
        /// ClientHandler class constructor
        /// </summary>
        /// <param name="client">object implementing the TCPCient interface (client)</param>
        public ClientHandler(TcpClient client)
        {
            //this.clientsAndStreams = new Dictionary<TcpClient, NetworkStream>();
            this.client = client;
            this.stream = this.client.GetStream();
            this.reader = new BinaryReader(this.stream);
            this.writer = new BinaryWriter(this.stream);
            this.running = true;
        }

        /// <summary>
        /// starting communication from client side with the server
        /// </summary>
        public void Start()
        {
            if (running)
            {
                new Task(() =>
                {
                    try
                    {
                        //this.clientsAndStreams.Add(client, stream);

                        string receivedString;
                        while (this.running)
                        {
                            lock (reader)
                            {
                                receivedString = reader.ReadString();
                                MessageReceived?.Invoke(this, new MessageCommunicationEventArgs { Message = receivedString });
                            }
                        }

                    }
                    catch (Exception exc)
                    {
                        //if communication didn't succedd close clientHandler
                        this.CloseHandler();

                    }
                }).Start();
            }

        }

        /// <summary>
        /// Close the handler
        /// </summary>
        public void CloseHandler()
        {
            if (this.running)
            {
                this.ClosingClientHandler?.Invoke(this, new IClientHandlerCloseEventArgs { Message = "closing client handler" });
                this.running = false;
                this.stream.Close();
                this.reader.Close();
                this.writer.Close();
                client.Close();
            }
        }

        /// <summary>
        /// write message to client
        /// </summary>
        /// <param name="message">object implementing the TCPCient interface (message)</param>
        public void WriteMessage(string message)
        {
            if (running)
            {
                lock (this.writer)
                {
                    try
                    {
                        writer.Write(message);
                    }
                    catch
                    {
                        this.CloseHandler();
                    }
                }

            }
        }
    }
}
