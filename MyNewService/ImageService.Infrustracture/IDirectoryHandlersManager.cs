using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.IDirecoryHandlersManager
{
    public interface IDirectoryHandlersManager
    {
        /// <summary>
        /// initiializing the directory handlers
        /// </summary>
        /// <param name="pathsToWatch">array of paths to directories needed to be watched</param>
        void InitDirectoryHandlers(string[] pathsToWatch);
        /// <summary>
        /// stop handeling certain directory
        /// </summary>
        /// <param name="directoryPath">path to te directory</param>
        /// <returns></returns>
        bool StopHandelingDirectory(string directoryPath);
        /// <summary>
        /// method for getting a list of the paths of directories being handled
        /// </summary>
        /// <returns>a list of the paths of directories being handled</returns>
        List<string> GetDirectoryHandlersPaths();
    }
}
