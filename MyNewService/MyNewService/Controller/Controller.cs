using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using Logging;
using Logging.Modal;

namespace ImageService.Controller
{
    /// <summary>
    /// class implementing the IController interface
    /// </summary>
    public class Controller:IController
    {
        private IImageModal imageModal;  // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;  //dictionary of the command objects this controller can handle
        private ILoggingService logger;  //logger service object

        /// <summary>
        /// Controller class constructor
        /// </summary>
        /// <param name="newModal">object implementing the IImageModal interface</param>
        /// <param name="log">object implementing the ILoggingService interface (logger service)</param>
        public Controller(IImageModal newModal, ILoggingService log)
        {
            imageModal = newModal;
            logger = log;
            commands = new Dictionary<CommandEnum, ICommand>() { };
            commands[CommandEnum.NewFileCommand] = new NewFileCommand(newModal);
            logger.Log("In Controller, finished constructor", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// Method for requesting the controller object to execute specific command
        /// </summary>
        /// <param name="commandID">the id of the command</param>
        /// <param name="args">arguments for the command execution request</param>
        /// <param name="result">the result of the execution</param>
        /// <returns>string indicating of success/failure</returns>
        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool result)
        {
            logger.Log("In controller, received command execution request with id: " + commandID, MessageTypeEnum.INFO);
            return commands[commandID].Execute(args, out result);
        }

    }
}
