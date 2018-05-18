using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandlersManager
    {
        void InitDirectoryHandlers(string[] pathsToWatch);
        bool StopHandelingDirectory(string directoryPath);
    }
}
