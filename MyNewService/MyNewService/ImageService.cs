using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Modal;
using ImageService.Server;
using System.Configuration;
using ImageService.Communication;
using ImageService.Infrastructure;

namespace ImageService
{

    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        //the server created by this service. saved for notifying the system of service shutdown
        private ImageServer server = null;
        
        /// <summary>
        /// ImageService constructor
        /// </summary>
        /// <param name="args">arguments to the service</param>
        public ImageService(string[]args)
        {
            InitializeComponent();

            //reading from the configuration file
            var appSettings = ConfigurationManager.AppSettings;
            //after ?? are the default values if there is no reference for them in the app config file
            string eventSourceName = appSettings["SourceName"] ?? "MySource";
            string logName = appSettings["LogName"] ?? "MyNewLoggger";
 
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            eventLog1.WriteEntry("Service Created");

        }

        /// <summary>
        /// method to write to the event logger of the service
        /// </summary>
        /// <param name="sender">message sender</param>
        /// <param name="messageArgs">info about the message sent</param>
        private void WriteToEventLogger(object sender, MessageRecievedEventArgs messageArgs)
        {
            string messageOpenning = "";
            switch (messageArgs.Status)
            {
                case MessageTypeEnum.FAIL:
                    messageOpenning += "FAILED: ";
                    break;
                case MessageTypeEnum.INFO:
                    messageOpenning += "INFO: ";
                    break;
                case MessageTypeEnum.WARNING:
                    messageOpenning += "WARNING: ";
                    break;
                default:
                    messageOpenning += "UNKNOWN MESSAGE TYPE: ";
                    break;
            }
            this.eventLog1.WriteEntry(messageOpenning += messageArgs.Message);
        }

        protected override void OnStart(string[] args)
        {

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In OnStart");

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //creating the Logger object used in the system
            ILoggsRecorder logger = new LoggingService();
            logger.MessageRecieved += this.WriteToEventLogger;
            logger.Log("In ImageService created the logging service", MessageTypeEnum.INFO);

            //creating the modal with app config paramaters
            logger.Log("In ImageService starting to create the Modal, Controller and Server", MessageTypeEnum.INFO);
            var appSettings = ConfigurationManager.AppSettings;
            string outputDir = appSettings["OutputDir"];
            string[] dirsToBeHandled = appSettings["Handler"].Split(';');
            int thumbnailSize;
            //if conversion failed, put default value
            if (!Int32.TryParse(appSettings["ThumbnailSize"], out thumbnailSize))
                thumbnailSize = 120;

            IImageModal modal = new ImageModal(logger, outputDir, thumbnailSize);
            IController controller = new Controller.Controller(modal, logger);

            IServerChannel serverChannel = new TcpServerChannel(8080);
            //!!!!!!!!!!!!!!!!!!!! subscribing to the: new log notifying event, so that the server can be notified of new logs !!!!!!!!!!!!!!!!!!!!!!!! also possible to connect the event to the imageServer or to the clientHandler, think whats better !!!!!!!!!!!!!!!!!!!!!!!!!!
            logger.MessageRecieved += serverChannel.NotifyServerOfMessage;
            server = new ImageServer(controller, logger, serverChannel, dirsToBeHandled);
            controller.SetDHManager(server);
            logger.Log("In ImageService finished creating the Modal, Controller and Server", MessageTypeEnum.INFO);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In On Stop");
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //notifying the system of service shutdown
            server.CloseServer();

            eventLog1.WriteEntry("Service stoppes");
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Service In OnContinue.");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
