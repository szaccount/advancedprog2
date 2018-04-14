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
   public  class Controller:IController
    {
        private IImageModal imageModal;                      // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;
        private ILoggingService logger;
        
        public Controller(IImageModal newModal, ILoggingService log)
        {
            imageModal = newModal;
            logger = log;
            commands = new Dictionary<CommandEnum, ICommand>() { };
            commands[CommandEnum.NewFileCommand] = new NewFileCommand(newModal); //add enum for the commands in the Commands directory and work with enum instead of with ints!!!!!!!!!!!!!!!!!!!
            logger.Log("In Controller, finished constructor", MessageTypeEnum.INFO);
        }

        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool result)
        {
            logger.Log("In controller, received command execution request with id: " + commandID, MessageTypeEnum.INFO);
            return commands[commandID].Execute(args, out result); //solve the case when the command ID is invalid (the command doesn't exist)!!!!!
        }

    }
}
