using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller
{
   public  class Controller:IController
    {
        private IImageModal imageModal;                      // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;
        //private ILogger logger; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        Controller(IImageModal newModal)
        {
            imageModal = newModal;
            commands = new Dictionary<CommandEnum, ICommand>() { };
            commands[CommandEnum.NewFileCommand] = new NewFileCommand(newModal); //add enum for the commands in the Commands directory and work with enum instead of with ints!!!!!!!!!!!!!!!!!!!
        }

        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool result)
        {
            return commands[commandID].Execute(args, out result); //solve the case when the command ID is invalid (the command doesn't exist)!!!!!
        }

    }
}
