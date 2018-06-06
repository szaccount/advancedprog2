using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ImageService.Communication
{
    /// <summary>
    /// interface for a clientHandler (handeling communication with client)
    /// </summary>
    public interface IClientHandler
    {
        void Start();
        void WriteMessage(string message);
        void CloseHandler();
        //notifying of new message arrival
        event EventHandler<MessageCommunicationEventArgs> MessageReceived;
        //notifying of handler closing
        event EventHandler<IClientHandlerCloseEventArgs> ClosingClientHandler;
    }
}
