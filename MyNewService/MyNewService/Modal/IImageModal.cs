using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
namespace ImageService.Modal
{
    public interface IImageModal
    {
        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string AddFile(string[] args, out bool result);

        /// <summary>
        /// Setter function to set up the logger object for the IImageModal
        /// </summary>
        void SetUpLogger(ILoggingService log);

    }
}
