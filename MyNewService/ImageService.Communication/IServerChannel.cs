using System;
using System.Collections.Generic;
using System.Text;
using ImageService.Logging.Modal;

namespace ImageService.Communication
{
    public interface IServerChannel
    {
        void Start();

        void Stop();

        //notifying the server of something in its surounding system, used to notify the clients in the clientsHandler of the loggs in the system.
        void NotifyServerOfMessage(object sender, MessageRecievedEventArgs messageArgs);
        void BroadcastToClients(ServerClientCommunicationCommand command);
        event EventHandler<MessageCommunicationEventArgs> MessageReceived;
    }
}
