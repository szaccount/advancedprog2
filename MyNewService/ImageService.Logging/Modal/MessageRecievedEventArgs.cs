using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{ 
    /// <summary>
    /// class representing the message arguments in the system
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        //the type of message
        public MessageTypeEnum Status { get; set; }
        //the message itself
        public string Message { get; set; }
    }
}
