using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class Controller
    {
        private IImageModal modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        //private ILogger logger;
        
        Controller(IImageModal newModal)
        {
            modal = newModal;
            commands = new Dictionary<int, ICommand>() { };
            commands[0] = new NewFileCommand(newModal);
        }

        public string ExcuteCommand(int commandID, string[] args, out bool result)
        {
            return commands[commandID].Execute(args, out result);
        }
    }
}
