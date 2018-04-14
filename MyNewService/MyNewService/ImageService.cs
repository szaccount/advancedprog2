﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Logging;
using Logging.Modal;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Modal;
using ImageService.Server;

namespace ImageService
{

    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;
        
        public ImageService(string[]args)
        {
            InitializeComponent();
            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            eventLog1.WriteEntry("Start Pending");

        }

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

            //creating the Logger!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ILoggingService logger = new LoggingService();
            logger.MessageRecieved += this.WriteToEventLogger;

            //creating the modal with app config paramaters
            //!!!!!!!!!!!!!!!!! 13.4.18 initializing the objects with default values !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            logger.Log("In ImageService starting to create the Modal, Controller and Server", MessageTypeEnum.INFO);
            IImageModal modal = new ImageModal(logger, @"D:\Users\seanz\Desktop\AdvancedProg2Work\SaveGallery", 120);
              
             IController controller = new Controller.Controller(modal, logger);
                                       
             ImageServer server = new ImageServer(controller, logger, @"D:\Users\seanz\Desktop\AdvancedProg2Work\listening1",
                 @"D:\Users\seanz\Desktop\AdvancedProg2Work\listening2");
            logger.Log("In ImageService finished creating the Modal, Controller and Server", MessageTypeEnum.INFO);
            //!!!!!!!!!!!!!!!!! 13.4.18 initializing the objects with default values !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("On Stop");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
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
