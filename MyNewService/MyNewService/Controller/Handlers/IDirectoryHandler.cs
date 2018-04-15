using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// interface for the Directory handlers. specifying the properties of a directory handling calss
    /// </summary>
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    // The Event That Notifies that the Directory is being closed
        
        /// <summary>
        /// method that receives path to a directory to be watched and initializes all the needed parts to do so
        /// </summary>
        /// <param name="dirPath">the path to the directory needed to be watched</param>
        void StartHandleDirectory(string dirPath);
        
        /// <summary>
        /// method handling commands being sent to the directory handler object
        /// </summary>
        /// <param name="sender">the sender of the command</param>
        /// <param name="e">arguments and info of the sent command</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
    }
}
