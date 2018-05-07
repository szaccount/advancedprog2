using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Modal.Event
{
    /// <summary>
    /// class representing the Command arguments in the system
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// the id of the command
        /// </summary>
        public CommandEnum CommandID { get; set; }

        /// <summary>
        /// the arguments of the command
        /// </summary>
        public string[] Args { get; set; }

        /// <summary>
        /// the requested direcotry path for the command
        /// </summary>
        public string RequestDirPath { get; set; }

        /// <summary>
        /// CommandRecievedEventArgs class constructor
        /// </summary>
        /// <param name="id">command id</param>
        /// <param name="args">command's arguments</param>
        /// <param name="path">directory path, used by the command</param>
        public CommandRecievedEventArgs(CommandEnum id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
