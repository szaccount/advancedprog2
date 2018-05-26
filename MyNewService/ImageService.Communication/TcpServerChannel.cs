using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;
using System.IO;
using ImageService.Infrustracture.ToFile;
using Newtonsoft.Json;

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
            this.running = false;
            this.port = port;
            clients = new List<IClientHandler>();
        }

        public void NotifyServerOfMessage(object sender, MessageRecievedEventArgs messageArgs)
        {
            //LoggerToFile.Logm("break1");
            if (messageArgs != null)
            {
                //LoggerToFile.Logm("break2");
                string[] messageArr = new string[1];
                List<MessageRecievedEventArgs> messageList = new List<MessageRecievedEventArgs>();
                messageList.Add(messageArgs);
                //LoggerToFile.Logm("break3");
                //saving the message
                messageArr[0] = JsonConvert.SerializeObject(messageList, Formatting.Indented); // !!!!!!!! maybe create an object of list of logs serializer and also use it in GetLoggsCommand 26.5 !!!!!!!!!!!!!!!!!!!!
                //LoggerToFile.Logm("break4");
                //saving the status
                //LoggerToFile.Logm("break5");
                //LoggerToFile.Logm("break6");
                BroadcastToClients(new ServerClientCommunicationCommand(CommandEnum.LogCommand, messageArr));
                //LoggerToFile.Logm("breakEnd");
            }
        }

        private void HandleMessage(object sender, MessageCommunicationEventArgs message)
        {
            //LoggerToFile.Logm("In tcpServerChannel received string to handle passing higher");
            this.MessageReceived?.Invoke(sender, message);
        }

        public void BroadcastToClients(ServerClientCommunicationCommand command)
        {
            if (running)
            {
                //LoggerToFile.Logm("break8");
                new Task(() =>
                {
                    if (running)
                    {
                        string commandJson = command.ToJson();
                        //LoggerToFile.Logm("break9");
                        foreach (IClientHandler clientHandler in clients)
                        {
                            //LoggerToFile.Logm("break10");
                            try
                            {
                                //LoggerToFile.Logm("break11");
                                clientHandler.WriteMessage(commandJson);
                                //LoggerToFile.Logm("break12");
                            }
                            catch (Exception)
                            {
                                //LoggerToFile.Logm("break13");
                                //if communication didn't succedd erase client from communication clients list ##########################
                                clientHandler.CloseHandler();
                                //LoggerToFile.Logm("break14");
                            }
                        }
                    }
                }).Start();
                //LoggerToFile.Logm("break8.5");
            }
        }

        public void Start()
        {
            if (!this.running)
            {
                this.running = true;
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); //לבדוק אם צריך לקבל את ה ip של המחשב
                listener = new TcpListener(ep);
                listener.Start();
                //!!!!!!!!!!! logger.Log("Waiting for client connections... in tcp server", Logging.Modal.MessageTypeEnum.INFO);
                Task task = new Task(() =>
                {
                    while (running)
                    {
                        try
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            IClientHandler ch = new ClientHandler(client);///"Factory!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
                            ch.Start();
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
