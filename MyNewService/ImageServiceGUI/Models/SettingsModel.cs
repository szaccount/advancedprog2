using ImageService.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ImageService.Infrastructure.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ImageService.Infrustracture.ToFile;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Threading;

namespace ImageServiceGUI.Models
{
    public class SettingsModel: INotifyPropertyChanged
    {
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

        public SettingsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
            commChannel.MessageReceived += ReadRecivedMessage;

            //LoggerToFile.Logm("In GUI asking config");
            this.GetConfig();
            //LoggerToFile.Logm("In GUI asked config");
            Thread.Sleep(1000); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public void GetConfig()
        {
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.GetConfigCommand, null);
            this.commChannel.Write(commCommand.ToJson());
        }

        private void ReadRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            string message = messageArgs.Message;
            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
            switch (commCommand.CommId)
            {
                case CommandEnum.GetConfigCommand:
                    //LoggerToFile.Logm("In GUI got config");
                    this.InitializeConfigData(commCommand.Args[0]);
                    break;
                case CommandEnum.CloseCommand:
                    this.ExecuteRemoveDirectoryPath(commCommand.Args[0]);
                    break;
                default:
                    break;
            }
        }

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

        private void ExecuteRemoveDirectoryPath(string directoryPath)
        {
            if (this.DirectoryHandlerPaths.Contains(directoryPath))
                try //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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

        public void SendRequestRemoveDirectoryPath(string path)
        {
            string[] args = new string[1];
            args[0] = path;
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.CloseCommand, args);
            this.commChannel.Write(commCommand.ToJson());
        }



        // delegate command of erasing a directory handlers after choosing path and clicking button !!!!!!!!!!!!!!!!!
    }
}
