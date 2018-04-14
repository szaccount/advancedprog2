using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal.Event;
using Logging.Modal;

namespace ImageService.Server
{
    //###############################################################################################################
    public class ImageServer
    {
        #region Members
        private IController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        // ???? (original description for this event:) The event that notifies about a new Command being recieved ????
        // !!!!!!!!!!!!!!!!!! shouldn't it be only for closing the Directory Handlers? what other commands can there be? !!!!!!!!!!!!!!!!!!!!!!!
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; // !!!!!!!! change it to close args specific and not just some command
        #endregion

        public ImageServer(IController controller, ILoggingService logger, string firstPathToWatch, string secondPathToWatch)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            m_logging.Log("in server constructor starting creating directory handlers", MessageTypeEnum.INFO);

            IDirectoryHandler directoryHandler1 = new DirectoyHandler(controller, logger);
            directoryHandler1.StartHandleDirectory(firstPathToWatch);
            this.CommandRecieved += directoryHandler1.OnCommandRecieved;
            directoryHandler1.DirectoryClose += this.RemoveDirectoryHandler;

            IDirectoryHandler directoryHandler2 = new DirectoyHandler(controller, logger);
            directoryHandler2.StartHandleDirectory(secondPathToWatch);
            this.CommandRecieved += directoryHandler2.OnCommandRecieved;
            directoryHandler2.DirectoryClose += this.RemoveDirectoryHandler;
            m_logging.Log("in server constructor finished creating directory handlers", MessageTypeEnum.INFO);
        }

        public void CloseServer()
        {
            this.m_logging.Log("starting closing server", MessageTypeEnum.INFO);
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs(CommandEnum.CloseCommand, null, "");
            this.CommandRecieved?.Invoke(this, commandRecievedEventArgs);
        }

        private void RemoveDirectoryHandler(object sender, DirectoryCloseEventArgs messageArgs)
        {
            IDirectoryHandler sendingDirectoryHandler = sender as IDirectoryHandler;
            if (sendingDirectoryHandler == null)
            {
                //an object that isn't supposed to activate the event did it
                this.m_logging.Log("unotherized object tried to announce about closing of directory in path: " + messageArgs.DirectoryPath, MessageTypeEnum.WARNING);
                return;
            }
            this.m_logging.Log("Directory Handler of Directory in: " + messageArgs.DirectoryPath + @" with message: " + messageArgs.Message, MessageTypeEnum.INFO);
            //unsubscribing of the DirectoryHandler from the server message feed
            this.CommandRecieved -= sendingDirectoryHandler.OnCommandRecieved;
            if (this.CommandRecieved == null)
                //if all the Directory Handlers closed succefully the server itself can finally close
                this.StopServerFinal();
        }

        //activates after all the Directory Handlers terminate
        private void StopServerFinal()
        {
            this.m_logging.Log("closing server finally", MessageTypeEnum.INFO);
        }
       
    }
}
