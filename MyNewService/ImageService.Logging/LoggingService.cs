
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// class implementing the ILoggingService interface
    /// </summary>
    public class LoggingService : ILoggsRecorder
    {
        private List<MessageRecievedEventArgs> m_receivedLoggsList;

        public LoggingService()
        {
            this.m_receivedLoggsList = new List<MessageRecievedEventArgs>();
        }

        //message distributing event
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Method for logging the Message
        /// </summary>
        /// <param name="message">the message itself</param>
        /// <param name="type">message's type</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs messageRecievedEventArgs = new MessageRecievedEventArgs
            {
                Message = message,
                Status = type
            };
            MessageRecievedEventArgs messageRecord = new MessageRecievedEventArgs
            {
                Message = message,
                Status = type
            };
            //if there are subscribers send them the message
            this.MessageRecieved?.Invoke(this, messageRecievedEventArgs);
            this.m_receivedLoggsList.Add(messageRecord);
        }

        /// <summary>
        /// method for getting the logs recorded
        /// </summary>
        /// <returns>the recorded logs</returns>
        public List<MessageRecievedEventArgs> GetLoggsRecord()
        {
            return new List<MessageRecievedEventArgs>(this.m_receivedLoggsList);
        }

    }
}
