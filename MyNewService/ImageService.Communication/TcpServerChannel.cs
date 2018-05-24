using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;
using System.IO;

namespace ImageService.Communication
{
    public class TcpServerChannel: IServerChannel
    {
        private List<IClientHandler> clients;
        private int port;
        private TcpListener listener;
        private bool running;
        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;
        public TcpServerChannel(int port)
        {
            this.port = port;
            clients = new List<IClientHandler>();
        }

        public void NotifyServerOfMessage(object sender, MessageRecievedEventArgs messageArgs)
        {
            if (messageArgs != null)
            {
                string[] messageArr = new string[1];
                //saving the message
                messageArr[0] = messageArgs.Message;
                //saving the status
                int statusIntValue = (int) messageArgs.Status;
                messageArr[1] = statusIntValue.ToString();
                BroadcastToClients(new ServerClientCommunicationCommand(CommandEnum.LogCommand, messageArr));
            }
        }

        private void HandleMessage(object sender, MessageCommunicationEventArgs message)
        {
            this.MessageReceived?.Invoke(sender, message);
        }

        public void BroadcastToClients(ServerClientCommunicationCommand command)
        {
            if (running)
            {
                new Task(() =>
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
                        //if communication didn't succedd erase client from communication clients list ##########################
                        clientHandler.CloseHandler();
                        }
                    }
                }).Start();
            }
        }

        public void Start()
        {
            this.running = true;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); //לבדוק אם צריך לקבל את ה ip של המחשב
            listener = new TcpListener(ep);
            listener.Start();
            //!!!!!!!!!!! logger.Log("Waiting for client connections... in tcp server", Logging.Modal.MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (running)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        IClientHandler ch = new ClientHandler(client);///"Factory!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
                        ch.MessageReceived += this.HandleMessage;
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
