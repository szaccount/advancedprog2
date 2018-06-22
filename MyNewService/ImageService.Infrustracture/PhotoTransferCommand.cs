using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using static System.Net.Mime.MediaTypeNames;
using ImageService.Infrastructure.IDirecoryHandlersManager;

namespace ImageService.Infrastructure.PhotoTransfer
{
    public class PhotoTransferCommand: ICommand
    {
        private IDirectoryHandlersManager directoryHandlersManager;
        private List<string> paths;
        public PhotoTransferCommand(IDirectoryHandlersManager dhm)
        {
            directoryHandlersManager = dhm;
            paths = new List<string>();
        }
        /// <summary>
        /// method for executing the command convert string to Image byte
        /// </summary>
        /// <param name="args">command's arguments</param>
        /// <param name="result">the result of command execution</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                byte[] imageByteArray = Convert.FromBase64String(args[1]);
                //byte[] imageByteArray = Encoding.ASCII.GetBytes(args[1]);
                MemoryStream memoryStream = new MemoryStream(imageByteArray);
                System.Drawing.Image image = System.Drawing.Image.FromStream(memoryStream);
                string name = this.paths[0] + @"\" + args[0];
                image.Save(name, ImageFormat.Png);
                result = true;
                return "success";
            }
            catch(Exception exc) {
                result = false;
                return "failure";
            }
        }

        /// <summary>
        /// method for setting the directoryHandlers manager
        /// </summary>
        /// <param name="manager">the directoryHandlers manager</param>
        public void SetDirectoryHandlersManager(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
            this.paths = this.directoryHandlersManager.GetDirectoryHandlersPaths();
        }
    }
}
