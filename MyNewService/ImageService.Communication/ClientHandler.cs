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
    public class ClientHandler : IClientHandler
    {

        private Dictionary<TcpClient, NetworkStream> clientsAndStreams;
        private bool running;
        private ICommandExecuter commandExecuter;

        public ClientHandler(ICommandExecuter controller)
        {
            this.clientsAndStreams = new Dictionary<TcpClient, NetworkStream>();
            this.running = true;
            this.commandExecuter = controller;
        }

        public void HandleClient(TcpClient client)
        {

            new Task(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    this.clientsAndStreams.Add(client, stream);

                    string receivedString;
                    using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        while (this.running)
                        {
                            receivedString = reader.ReadLine();
                            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(receivedString);
                            switch (commCommand.CommId) //!!!!!!!!!!!!!!!!!! check on the command if they failed (using the out bool variable) and if so send back informative message !!!!!!!!!!!!!
                            {
                                case CommandEnum.CloseGuiClient:
                                    this.CloseClient(client);
                                    break;
                                case CommandEnum.GetConfigCommand:
                                    //asking for logs list
                                case CommandEnum.LogCommand:
                                    string result = this.commandExecuter.ExecuteCommand(commCommand.CommId, commCommand.Args, out bool flag1);
                                    string[] responseArr1 = new string[1];
                                    responseArr1[0] = result;
                                    ServerClientCommunicationCommand responseCommand = new ServerClientCommunicationCommand(commCommand.CommId, responseArr1);
                                    string responseJson = responseCommand.ToJson();
                                    //writing back the answer to the client request
                                    writer.Write(responseJson);
                                    break;
                                    //closing a directory handler request
                                case CommandEnum.CloseCommand:
                                    string pathRemoved = this.commandExecuter.ExecuteCommand(commCommand.CommId, commCommand.Args, out bool flag2);
                                    //writing to all of the connected clients the path of directory which's handler closed
                                    string[] responseArr2 = new string[1];
                                    responseArr2[0] = pathRemoved;
                                    this.BroadcastToClients(new ServerClientCommunicationCommand(CommandEnum.CloseCommand, responseArr2));
                                    break;
                                default:
                                    writer.Write("Invalid command Id");
                                    break;

                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //if communication didn't succedd erase client #########################
                    this.CloseClient(client);

                }
            }).Start();

        }

        public void BroadcastToClients(ServerClientCommunicationCommand command)
        {
            new Task(() =>
            {
                string commandJson = command.ToJson();
                foreach (KeyValuePair<TcpClient, NetworkStream> entry in clientsAndStreams)
                {
                    try
                    {
                        NetworkStream stream = entry.Value;
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(commandJson);
                        }
                    }
                    catch (Exception)
                    {
                        //if communication didn't succedd erase client from communication clients list ##########################
                        this.CloseClient(entry.Key);
                    }
                }
            }).Start();
        }

        public void CloseHandler()
        {
            this.running = false;
            foreach (KeyValuePair<TcpClient, NetworkStream> entry in clientsAndStreams)
            {
                entry.Key.Close();
            }
            clientsAndStreams.Clear();
        }

        private void CloseClient(TcpClient client)
        {
            if (this.running)
            {
                //if the handler isn't running anymore the client will be already closed in the handler closing method
                client.Close();
                if (this.clientsAndStreams.ContainsKey(client))
                    this.clientsAndStreams.Remove(client);
            }
        }
    }
}
