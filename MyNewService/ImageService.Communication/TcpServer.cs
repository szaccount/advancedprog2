using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Logging;

namespace ImageService.Communication
{
    class TcpServer
    {
        private ILoggingService logger;
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        public TcpServer(int port, IClientHandler ch, ILoggingService log)
        {
            this.port = port;
            this.ch = ch;
            this.logger = log;
        }
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); //לבדוק אם צריך לקבל את ה ip של המחשב
            listener = new TcpListener(ep);
            listener.Start();
            logger.Log("Waiting for client connections... in tcp server", Logging.Modal.MessageTypeEnum.INFO);
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        logger.Log("Got new client connection in tcp server", Logging.Modal.MessageTypeEnum.INFO);
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }
    }
}
