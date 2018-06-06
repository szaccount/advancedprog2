using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ImageService.Infrastructure.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Threading;
using ImageService.Communication;

namespace ImageServiceGUI.Models
{
    /// <summary>
    /// model for the settings MVVM
    /// </summary>
    public class SettingsModel: ISettingsModel, INotifyPropertyChanged
    {
        //communiation channel
        private TcpClientChannel commChannel;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string outputDirectory;
        private string sourceName;
        private string logName;
        private string thumbnailSize;
        /// <summary>
        /// collection od the directoryHandlers directory paths
        /// </summary>
        private ObservableCollection<string> directoryHandlerPaths;

        public string OutputDirectory
        {
            get
            {
                return this.outputDirectory;
            }
            set
            {
                if (this.outputDirectory != value)
                {
                    this.outputDirectory = value;
                    NotifyPropertyChanged("OutputDirectory");
                }
            }
        }

        public string SourceName
        {
            get
            {
                return this.sourceName;
            }
            set
            {
                if (this.sourceName != value)
                {
                    this.sourceName = value;
                    NotifyPropertyChanged("SourceName");
                }
            }
        }

        public string LogName
        {
            get
            {
                return this.logName;
            }
            set
            {
                if (this.logName != value)
                {
                    this.logName = value;
                    NotifyPropertyChanged("LogName");
                }
            }
        }

        public string ThumbnailSize
        {
            get
            {
                return this.thumbnailSize;
            }
            set
            {
                if (this.thumbnailSize != value)
                {
                    this.thumbnailSize = value;
                    NotifyPropertyChanged("ThumbnailSize");
                } 
            }
        }

        /// <summary>
        /// collection od the directoryHandlers directory paths Property
        /// </summary>
        public ObservableCollection<string> DirectoryHandlerPaths
        {
            get
            {
                return this.directoryHandlerPaths;
            }
            set
            {
                if (this.directoryHandlerPaths != value)
                {
                    this.directoryHandlerPaths = value;
                    NotifyPropertyChanged("DirectoryHandlerPaths");
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public SettingsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
            commChannel.MessageReceived += ReadRecivedMessage;

            this.GetConfig();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// method to request the config values
        /// </summary>
        public void GetConfig()
        {
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.GetConfigCommand, null);
            this.commChannel.Write(commCommand.ToJson());
        }

        /// <summary>
        /// method to handle received message
        /// </summary>
        /// <param name="sender">the sender of the message</param>
        /// <param name="messageArgs">the arguments of the message</param>
        private void ReadRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            string message = messageArgs.Message;
            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
            switch (commCommand.CommId)
            {
                case CommandEnum.GetConfigCommand:
                    this.InitializeConfigData(commCommand.Args[0]);
                    break;
                case CommandEnum.CloseCommand:
                    this.ExecuteRemoveDirectoryPath(commCommand.Args[0]);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// method to initialize config data
        /// </summary>
        /// <param name="jsonData">the data in json form</param>
        private void InitializeConfigData(string jsonData)
        {
            JObject appConfigData = JObject.Parse(jsonData);
            OutputDirectory = (string)appConfigData["OutputDir"];
            SourceName = (string)appConfigData["SourceName"];
            LogName = (string)appConfigData["LogName"];
            ThumbnailSize = (string)appConfigData["ThumbnailSize"];
            string dirPathsListString = (string)appConfigData["dirPathsToManageListString"];
            DirectoryHandlerPaths = new ObservableCollection<string>(JsonConvert.DeserializeObject<List<string>>(dirPathsListString));

        }

        /// <summary>
        /// method to remove the path of the removed directoryHandler
        /// </summary>
        /// <param name="directoryPath"></param>
        private void ExecuteRemoveDirectoryPath(string directoryPath)
        {
            if (this.DirectoryHandlerPaths.Contains(directoryPath))
                try
                {
                    //moving the action to be handled in the UI thread
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.DirectoryHandlerPaths.Remove(directoryPath);
                    });
                }
                catch (Exception exc)
                {
                    string msg = exc.Message;
                } 
        }
        /// <summary>
        /// method to send request of removing a directory handler based on a directory path
        /// </summary>
        /// <param name="path">the path of the directory that the handler listens to</param>
        public void SendRequestRemoveDirectoryPath(string path)
        {
            string[] args = new string[1];
            args[0] = path;
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.CloseCommand, args);
            this.commChannel.Write(commCommand.ToJson());
        }

    }
}
