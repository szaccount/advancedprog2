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
    /// client handler object for the photos transferring clients
    /// </summary>
    class ClientHandlerPhoto: IClientHandler
    {
        private bool running;
        private NetworkStream stream;
        private BinaryReader reader;
        private TcpClient client;

        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;

        public event EventHandler<IClientHandlerCloseEventArgs> ClosingClientHandler;

        /// <summary>
        /// ClientHandler class constructor
        /// </summary>
        /// <param name="client">object implementing the TCPCient interface (client)</param>
        public ClientHandlerPhoto(TcpClient client)
        {
            //this.clientsAndStreams = new Dictionary<TcpClient, NetworkStream>();
            this.client = client;
            this.stream = this.client.GetStream();
            this.reader = new BinaryReader(this.stream);
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
                        //this.clientsAndStreams.Add(client, stream
                        string receivedString;
                        while (this.running)
                        {
                            lock (reader) {
                                int nameLength = reader.ReadInt32();
                                string name = Encoding.Default.GetString(reader.ReadBytes(nameLength));
                                int imageLength = reader.ReadInt32();
                                byte[] image = reader.ReadBytes(imageLength);

                                string imageConvert = Convert.ToBase64String(image);
                                //string imageConvert = Encoding.ASCII.GetString(image);

                                string[] args = new string[2];
                                args[0] = name;
                                args[1] = imageConvert;
                                ServerClientCommunicationCommand command = new ServerClientCommunicationCommand() { CommId = CommandEnum.PhotoTransferCommand, Args = args };
                                MessageReceived?.Invoke(this, new MessageCommunicationEventArgs { Message = command.ToJson() });
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
                client.Close();
            }
        }

        /// <summary>
        /// write message to client
        /// </summary>
        /// <param name="message">object implementing the TCPCient interface (message)</param>
        public void WriteMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
