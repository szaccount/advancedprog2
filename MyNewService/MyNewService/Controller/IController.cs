using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure;
using ImageService.Controller.Handlers;

namespace ImageService.Controller
{
    /// <summary>
    /// interface specifying the properties of controller class
    /// </summary>
    public interface IController: ICommandExecuter
    {
        /// <summary>
        /// method for setting the directoryHandlers manager
        /// </summary>
        /// <param name="dhManager">the directoryHandlers manager</param>
        void SetDHManager(IDirectoryHandlersManager dhManager);
    }
}
