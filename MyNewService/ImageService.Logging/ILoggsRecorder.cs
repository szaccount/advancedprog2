﻿using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// interface for a loggs recorder class which can return a list of the past loggs in the system
    /// </summary>
    public interface ILoggsRecorder: ILoggingService
    {
        /// <summary>
        /// method for getting the logs recorded
        /// </summary>
        /// <returns>the recorded logs</returns>
        List<MessageRecievedEventArgs> GetLoggsRecord();
    }
}
