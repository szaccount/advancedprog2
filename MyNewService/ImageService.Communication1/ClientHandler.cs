﻿using System;
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

        
        private bool running;
        private NetworkStream stream;
        private TcpClient client;

        public event EventHandler<MessageCommunicationEventArgs> MessageReceived;

        public ClientHandler(TcpClient client)
        {
            //this.clientsAndStreams = new Dictionary<TcpClient, NetworkStream>();
            this.client = client;
            this.stream = this.client.GetStream();
            this.running = true;
        }

        public void Start()
        {

            new Task(() =>
            {
            try
            {
                //this.clientsAndStreams.Add(client, stream);

                string receivedString;
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    while (this.running)
                    {
                        receivedString = reader.ReadString();
                        MessageReceived?.Invoke(this, new MessageCommunicationEventArgs { Message = receivedString });
                            //!!!!!!ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(receivedString);
                            /*switch (commCommand.CommId) //!!!!!!!!!!!!!!!!!! check on the command if they failed (using the out bool variable) and if so send back informative message !!!!!!!!!!!!!
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

                            }*/
                        }
                    }
                }
                catch (Exception)
                {
                    //if communication didn't succedd erase client #########################
                    this.CloseHandler();

                }
            }).Start();

        }

        public void CloseHandler()
        {
            if (this.running)
            {
                //if the handler isn't running anymore the client will be already closed in the handler closing method
                this.stream.Close();
                client.Close();
                this.running = false;
            }
        }

        public void WriteMessage(string message)
        {
            if (running)
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(message);
                }
            }
        }
    }
}