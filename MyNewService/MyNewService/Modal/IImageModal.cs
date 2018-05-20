using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
namespace ImageService.Modal
{
    /// <summary>
    /// The ImageModal Interface
    /// </summary>
    public interface IImageModal
    {
        /// <summary>
        /// The Function Addes A file to the system (moves from the source to the needed destination),
        /// based on the received args
        /// args[0] = DirectoryPath to the image
        /// args[1] = picture name
        /// args[2] = year number
        /// args[3] = month number 
        /// </summary>
        /// <param name="args">The argumens (info about the file) for the transfer</param>
        /// <param name="result">the result of the function (true for success, false for error)</param>
        /// <returns>Indication if the Addition Was Successful ("success\failed")</returns>
        string AddFile(string[] args, out bool result);

        /// <summary>
        /// Setter function to set up the logger object for the IImageModal
        /// </summary>
        /// <param name="log">object implementing the ILoggingService interface (logger object)</param>
        void SetUpLogger(ILoggingService log);

    }
}
