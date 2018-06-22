using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure;
using ImageService.Infrastructure.PhotoTransfer;
using ImageService.Infrastructure.IDirecoryHandlersManager;



namespace ImageService.Controller
{
    /// <summary>
    /// class implementing the IController interface
    /// </summary>
    public class Controller:IController
    {
        private IImageModal imageModal;  // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;  //dictionary of the command objects this controller can handle
        private ILoggsRecorder logger;  //logger service object
        private IDirectoryHandlersManager directoryHandlersManager;

        /// <summary>
        /// Controller class constructor
        /// </summary>
        /// <param name="newModal">object implementing the IImageModal interface</param>
        /// <param name="log">object implementing the ILoggingService interface (logger service)</param>
        public Controller(IImageModal newModal, ILoggsRecorder log)
        {
            imageModal = newModal;
            logger = log;
            directoryHandlersManager = null;
            commands = new Dictionary<CommandEnum, ICommand>() { };
            commands[CommandEnum.NewFileCommand] = new NewFileCommand(this.imageModal);
            commands[CommandEnum.LogCommand] = new GetLoggsCommand(this.logger);
            commands[CommandEnum.CloseCommand] = new CloseDHandlerCommand(this.directoryHandlersManager);
            commands[CommandEnum.GetConfigCommand] = new GetConfigCommand(this.directoryHandlersManager);
            commands[CommandEnum.PhotoTransferCommand] = new PhotoTransferCommand(this.directoryHandlersManager);
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
            ICommand command;
            if (commands.TryGetValue(commandID, out command))
            {
                return command.Execute(args, out result);
            }
            else
            {
                result = false;
                return "";
            }
        }

        /// <summary>
        /// method for setting the directoryHandlers manager
        /// </summary>
        /// <param name="dhManager">the directoryHandlers manager</param>
        public void SetDHManager(IDirectoryHandlersManager dhManager)
        {
            this.directoryHandlersManager = dhManager;
            CloseDHandlerCommand command1 = this.commands[CommandEnum.CloseCommand] as CloseDHandlerCommand;
            command1?.SetDirectoryHandlersManager(this.directoryHandlersManager);
            GetConfigCommand command2 = this.commands[CommandEnum.GetConfigCommand] as GetConfigCommand;
            command2?.SetDirectoryHandlersManager(this.directoryHandlersManager);
            PhotoTransferCommand command3 = this.commands[CommandEnum.PhotoTransferCommand] as PhotoTransferCommand;
            command3?.SetDirectoryHandlersManager(this.directoryHandlersManager);
        }

        private List<string> GetDirectoryHandlersPathsFromDHManger()
        {
            if (this.directoryHandlersManager != null)
            {
                return this.directoryHandlersManager.GetDirectoryHandlersPaths();
            }
            return new List<string>();
        }
    }
}
