using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;
using System.IO;
using Newtonsoft.Json;

namespace ImageService.Communication
{
    /// <summary>
    /// implementing IServerChannel interface
    /// </summary>
    public class TcpServerChannel: IServerChannel
    {
        private List<IClientHandler> clients;
        private int port;
        private TcpListener listener;
        private bool running;
        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;

        /// <summary>
        /// class Constructor, init the clientHandler list and the port
        /// </summary>
        public TcpServerChannel(int port)
        {
            this.running = false;
            this.port = port;
            clients = new List<IClientHandler>();
        }

        /// <summary>
        /// function to notify the server channel that a message was received
        /// </summary>
        /// <param name="sender">the sender of message</param>
        /// <param name="messageArgs">object implementing the MessageRecievedEventArgs (a message)</param>
        public void NotifyServerOfMessage(object sender, MessageRecievedEventArgs messageArgs)
        {
            if (messageArgs != null)
            {
                string[] messageArr = new string[1];
                List<MessageRecievedEventArgs> messageList = new List<MessageRecievedEventArgs>();
                messageList.Add(messageArgs);
                messageArr[0] = JsonConvert.SerializeObject(messageList, Formatting.Indented);
                BroadcastToClients(new ServerClientCommunicationCommand(CommandEnum.LogCommand, messageArr));
            }
        }

        /// <summary>
        /// forwrds the receoved message higher in the heirarchy
        /// </summary>
        /// <param name="sender">the sender of the message</param>
        /// <param name="message">object implementing the  MessageRecievedEventArgs (a message)</param>
        private void HandleMessage(object sender, MessageCommunicationEventArgs message)
        {
            this.MessageReceived?.Invoke(sender, message);
        }

        /// <summary>
        /// broadcasting the received message to all the clients on the channel
        /// </summary>
        /// <param name="command">the message to broadcast</param>
        public void BroadcastToClients(ServerClientCommunicationCommand command)
        {
            if (running)
            {
                new Task(() =>
                {
                    if (running)
                    {
                        string commandJson = command.ToJson();
                        foreach (IClientHandler clientHandler in clients)
                        {
                            try
                            {
                                clientHandler.WriteMessage(commandJson);
                            }
                            catch (Exception)
                            {
                                //if communication didn't succedd close clientHandler
                                clientHandler.CloseHandler();
                            }
                        }
                    }
                }).Start();
            }
        }

        /// <summary>
        /// method to remove the IClientHandler object that notified of its closing
        /// </summary>
        /// <param name="sender">the closing IClientHandler</param>
        /// <param name="args">closing arguments</param>
        public void RemoveClosedIClientHandler(object sender, IClientHandlerCloseEventArgs args)
        {
            try
            {
                IClientHandler closingClientHandler = sender as IClientHandler;
                if (closingClientHandler != null)
                {
                    if (this.clients.Contains(closingClientHandler))
                        this.clients.Remove(closingClientHandler);
                }
            }
            catch (Exception) { }
        }
        /// <summary>
        /// starting the channel and listening for clients
        /// </summary>
        public void Start()
        {
            if (!this.running)
            {
                this.running = true;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                listener = new TcpListener(ep);
                listener.Start();
                Task task = new Task(() =>
                {
                    while (running)
                    {
                        try
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            IClientHandler ch = new ClientHandler(client);
                            ch.Start();
                            ch.MessageReceived += this.HandleMessage;
                            ch.ClosingClientHandler += RemoveClosedIClientHandler;
                            clients.Add(ch);
                        }
                        catch (SocketException)
                        {
                            break;
                        }
                    }
                });
                task.Start();
            }
        }

        /// <summary>
        /// stopping the channel, and closing the clients
        /// </summary>
        public void Stop()
        {
            this.listener.Stop();
            this.running = false;
            foreach (IClientHandler clientHandler in clients)
            {
                if (clients.Contains(clientHandler))
                {
                    clientHandler.CloseHandler();
                }
            }
            clients.Clear();
        }
    }
}
