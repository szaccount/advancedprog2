using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ImageService.Communication
{
    /// <summary>
    /// interface for a clientHandler wich saves in some way the clients it has communication with
    /// </summary>
    public interface IClientHandler
    {
        void HandleClient(TcpClient client);
        void BroadcastToClients(ServerClientCommunicationCommand command);
        void CloseHandler();
    }
}
