using ImageService.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ImageService.Infrastructure.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

        public string OutputDirectory { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<string> DirectoryHandlerPsths {get; set;}

        public SettingsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
            commChannel.MessageReceived += ReadRecivedMessage;
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
                    break;
                case CommandEnum.CloseCommand:
                    break;
                default:
                    break;
            }
        }

        // delegate command of erasing a directory handlers after choosing path and clicking button !!!!!!!!!!!!!!!!!
    }
}
