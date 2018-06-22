using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure;
using ImageService.Infrastructure.IDirecoryHandlersManager;

namespace ImageService.Commands
{
    public class CloseDHandlerCommand : ICommand
    {
        private IDirectoryHandlersManager directoryHandlersManager;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="manager">directoryHandlers manager</param>
        public CloseDHandlerCommand(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }

        /// <summary>
        /// exuting the command, assums the path to the directory is the first string in args
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result">indicating if action successful</param>
        /// <returns>return the path to the directory requested to be stopped</returns>
        public string Execute(string[] args, out bool result)
        {
            string directoryPath = args[0];
            result = directoryHandlersManager.StopHandelingDirectory(directoryPath);
            return directoryPath;
        }
        /// <summary>
        /// method for setting the directoryHandelrsManager of the command
        /// </summary>
        /// <param name="manager">directoryHandlers manager</param>
        public void SetDirectoryHandlersManager(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }
    }
}
