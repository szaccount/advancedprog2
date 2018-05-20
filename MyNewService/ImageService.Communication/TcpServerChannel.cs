using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;

namespace ImageService.Communication
{
    public class TcpServerChannel: IServerChannel
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;

        public TcpServerChannel(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
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
                this.ch?.BroadcastToClients(new ServerClientCommunicationCommand(CommandEnum.LogCommand, messageArr));
            }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); //לבדוק אם צריך לקבל את ה ip של המחשב
            listener = new TcpListener(ep);
            listener.Start();
            //!!!!!!!!!!! logger.Log("Waiting for client connections... in tcp server", Logging.Modal.MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        ch.HandleClient(client);
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
            this.ch.CloseHandler();
        }
    }
}
