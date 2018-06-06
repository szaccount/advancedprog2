using System;
using System.Collections.Generic;
using System.Text;
using ImageService.Logging.Modal;

namespace ImageService.Communication
{
    /// <summary>
    /// Interface representing server communication channel with clients
    /// </summary>
    public interface IServerChannel
    {
        /// <summary>
        /// start the channel
        /// </summary>
        void Start();

        /// <summary>
        /// stop the channel
        /// </summary>
        void Stop();

        /// <summary>
        /// notifying the server of something in its surounding system, used to notify of the loggs in the system.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="messageArgs">arguments</param>
        void NotifyServerOfMessage(object sender, MessageRecievedEventArgs messageArgs);
        /// <summary>
        /// broadcasting the command to all the clients on the channel
        /// </summary>
        /// <param name="command">command to broadcast</param>
        void BroadcastToClients(ServerClientCommunicationCommand command);
        /// <summary>
        /// notifying of arrival of a new message
        /// </summary>
        event EventHandler<MessageCommunicationEventArgs> MessageReceived;
    }
}
