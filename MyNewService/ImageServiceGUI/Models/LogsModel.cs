using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;
using Newtonsoft.Json;

namespace ImageServiceGUI.Models
{
    public class LogsModel : INotifyPropertyChanged
    {
        private TcpClientChannel commChannel;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<MessageRecievedEventArgs> Logs { get; set; }

        public LogsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
            commChannel.MessageReceived += ReadRecivedMessage;
        }

        public void GetLogs()
        {
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.LogCommand, null);
            this.commChannel.Write(commCommand.ToJson());
        }

        private void ReadRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            string message = messageArgs.Message;
            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
            switch (commCommand.CommId)
            {
                case CommandEnum.LogCommand:
                    string jsonLogs = commCommand.Args[0];
                    this.Logs.AddRange(JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(jsonLogs));
                    break;
                default:
                    break;
            }
        }
    }
}
