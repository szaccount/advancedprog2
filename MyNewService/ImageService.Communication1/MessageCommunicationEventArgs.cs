﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class MessageCommunicationEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
