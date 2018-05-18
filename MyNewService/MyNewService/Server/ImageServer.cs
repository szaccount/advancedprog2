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
using ImageService.Communication;
using ImageService.Infrastructure;

namespace ImageService.Server
{
    /// <summary>
    /// class defining the server object of the system
    /// </summary>
    public class ImageServer: IDirectoryHandlersManager
    {
        #region Members
        private IController m_controller;
        //the logger service of the server
        private ILoggingService m_logging;

        //the server itself
        private IServerChannel m_serverChannel;
        //map between directory paths and the handlers handeling them
        private Dictionary<string, IDirectoryHandler> m_deirectoryPathsToHandlers;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        /// <summary>
        /// the server class constructor
        /// </summary>
        /// <param name="controller">object implementing the IController interface</param>
        /// <param name="logger">object implementing the ILoggingService interface (logger service)</param>
        /// <param name="pathsToWatch">strings describing the paths of directories needed to be monitored by the system</param>
        public ImageServer(IController controller, ILoggingService logger, IServerChannel serverChannel, string[] pathsToWatch)
        {
            this.m_deirectoryPathsToHandlers = new Dictionary<string, IDirectoryHandler>();
            this.m_serverChannel = serverChannel;
            this.m_serverChannel.Start();

            this.m_controller = controller;
            this.m_logging = logger;
            m_logging.Log("in server constructor starting creating directory handlers", MessageTypeEnum.INFO);
            this.InitDirectoryHandlers(pathsToWatch);
            m_logging.Log("In server constructor finished creating directory handlers", MessageTypeEnum.INFO);
        }

        public void InitDirectoryHandlers(string[] pathsToWatch)
        {
            for (int i = 0; i < pathsToWatch.Length; i++)
            {
                IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_controller, this.m_logging);
                directoryHandler.StartHandleDirectory(pathsToWatch[i]);
                //adding handler and path to directory into dictionary
                this.m_deirectoryPathsToHandlers.Add(pathsToWatch[i], directoryHandler);
                this.CommandRecieved += directoryHandler.OnCommandRecieved;
                directoryHandler.DirectoryClose += this.RemoveDirectoryHandler;
            }
        }

        //returns true if directory stopped being handled and false otherwise
        public bool StopHandelingDirectory(string directoryPath)
        {
            if (this.m_deirectoryPathsToHandlers.ContainsKey(directoryPath))
            {
                IDirectoryHandler handlerToBeStopped = this.m_deirectoryPathsToHandlers[directoryPath];
                CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs(CommandEnum.CloseCommand, null, "");
                handlerToBeStopped.OnCommandRecieved(this, commandRecievedEventArgs);
                this.m_deirectoryPathsToHandlers.Remove(directoryPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// method for notifying the server of closing
        /// </summary>
        public void CloseServer()
        {
            this.m_logging.Log("starting closing server", MessageTypeEnum.INFO);
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs(CommandEnum.CloseCommand, null, "");
            this.CommandRecieved?.Invoke(this, commandRecievedEventArgs);
            this.m_serverChannel.Stop();
        }

        /// <summary>
        /// method for removing directory handler's method subscriber to the CommandRecieved event.
        /// When the event has no subscribers the server terminates
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="messageArgs">message rguments and info</param>
        private void RemoveDirectoryHandler(object sender, DirectoryCloseEventArgs messageArgs)
        {
            IDirectoryHandler sendingDirectoryHandler = sender as IDirectoryHandler;
            if (sendingDirectoryHandler == null)
            {
                //an object that isn't supposed to activate the event did it
                this.m_logging.Log("unotherized object tried to announce about closing of directory in path: " + messageArgs.DirectoryPath, MessageTypeEnum.WARNING);
                return;
            }
            this.m_logging.Log("In server, Closed Directory Handler of Directory in: " + messageArgs.DirectoryPath + @" with message: " + messageArgs.Message, MessageTypeEnum.INFO);
            //unsubscribing of the DirectoryHandler from the server message feed
            this.CommandRecieved -= sendingDirectoryHandler.OnCommandRecieved;
            if (this.CommandRecieved == null)
                //if all the Directory Handlers closed succefully the server itself can finally close
                this.StopServerFinal();
        }

        /// <summary>
        /// activates after all the Directory Handlers terminate(unsubscribe to the server command event)
        /// </summary>
        private void StopServerFinal()
        {
            this.m_logging.Log("closing server finally", MessageTypeEnum.INFO);
        }
       
    }
}
