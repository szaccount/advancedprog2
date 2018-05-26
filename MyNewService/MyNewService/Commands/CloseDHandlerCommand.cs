using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller.Handlers;

namespace ImageService.Commands
{
    public class CloseDHandlerCommand : ICommand
    {
        private IDirectoryHandlersManager directoryHandlersManager;

        public CloseDHandlerCommand(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }

        //assums the path to the directory is the first string in args
        //return the path to the directory requested to be stopped
        public string Execute(string[] args, out bool result)
        {
            string directoryPath = args[0];
            result = directoryHandlersManager.StopHandelingDirectory(directoryPath);
            return directoryPath;
        }

        public void SetDirectoryHandlersManager(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }
    }
}
