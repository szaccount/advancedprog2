using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class NewFileCommand:ICommand
    {
        private IImageModal modal;

        public NewFileCommand(IImageModal newModal)
        {
            this.modal = newModal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            return modal.AddFile(args, out result);
        }
    }
}
