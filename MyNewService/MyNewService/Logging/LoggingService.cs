
using Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    //#####################################################################################################################
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs messageRecievedEventArgs = new MessageRecievedEventArgs
            {
                Message = message,
                Status = type
            };
            //if there are subscribers send them the message
            this.MessageRecieved?.Invoke(this, messageRecievedEventArgs);
        }

    }
}
