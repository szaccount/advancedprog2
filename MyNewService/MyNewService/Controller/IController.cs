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
        void SetDHManager(IDirectoryHandlersManager dhManager);
    }
}
