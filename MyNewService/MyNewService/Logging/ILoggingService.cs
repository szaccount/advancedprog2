using Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    /// <summary>
    /// interface specifying the properties of logging service
    /// </summary>
    public interface ILoggingService
    {
        //message distributing event
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Method for logging the Message
        /// </summary>
        /// <param name="message">the message itself</param>
        /// <param name="type">message's type</param>
        void Log(string message, MessageTypeEnum type);
    }
}
