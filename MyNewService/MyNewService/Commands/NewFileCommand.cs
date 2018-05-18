using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;

namespace ImageService.Commands
{
    /// <summary>
    /// class implementing the ICommand interface (command class)
    /// </summary>
    class NewFileCommand: ICommand
    {
        //object of class implementing the IImageModal interface, for executing the command
        private IImageModal modal;

        /// <summary>
        /// NewFileCommand class constructor
        /// </summary>
        /// <param name="newModal">object implementing the IImageModal interface</param>
        public NewFileCommand(IImageModal newModal)
        {
            this.modal = newModal;
        }

        /// <summary>
        /// method for executing the command
        /// </summary>
        /// <param name="args">command's arguments</param>
        /// <param name="result">the result of command execution</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            return modal.AddFile(args, out result);
        }
    }
}
