using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
{
    /// <summary>
    /// class representing the message arguments in the system (for closing)
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        /// <summary>
        /// the path of the directory
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// attached message
        /// </summary>
        public string Message { get; set; }             // The Message That goes to the logger

        /// <summary>
        /// DirectoryCloseEventArgs class constructor
        /// </summary>
        /// <param name="dirPath">directory path</param>
        /// <param name="message">attached message</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;  // Setting the Directory Name
            Message = message;        // Storing the message
        }

    }
}
