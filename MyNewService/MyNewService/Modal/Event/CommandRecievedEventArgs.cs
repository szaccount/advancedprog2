using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Modal.Event
{
    //###############################################################################################################
    public class CommandRecievedEventArgs : EventArgs
    {
        public CommandEnum CommandID { get; set; }      // The Command ID // changed id from int to the CommandEnum type !!!!!!!!!!!!!!!!!!
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory

        public CommandRecievedEventArgs(CommandEnum id, string[] args, string path) // changed id from int to the CommandEnum type !!!!!!!!!!!!!!!!!!
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
