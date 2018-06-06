using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading;

namespace ImageServiceGUI.Models
{
    /// <summary>
    /// model for settings MVVM
    /// </summary>
    public class LogsModel : ILogsModel, INotifyPropertyChanged
    {
        //communication channel
        private TcpClientChannel commChannel;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MessageRecievedEventArgs> logs;
        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            get
            {
                return this.logs;
            }
            set
            {
                if (this.logs != value)
                {
                    this.logs = value;
                    NotifyPropertyChanged("Logs");
                }
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public LogsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
            commChannel.MessageReceived += ReadRecivedMessage;
            this.Logs = new ObservableCollection<MessageRecievedEventArgs>();
            Thread.Sleep(1000);
            this.GetLogs();
        }
        /// <summary>
        /// method to request to get the logs
        /// </summary>
        public void GetLogs()
        {
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.LogCommand, null);
            this.commChannel.Write(commCommand.ToJson());
        }

        /// <summary>
        /// method to handle a received message
        /// </summary>
        /// <param name="sender">sender of the message</param>
        /// <param name="messageArgs">the message arguments</param>
        private void ReadRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            string message = messageArgs.Message;
            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
            switch (commCommand.CommId)
            {
                case CommandEnum.LogCommand:
                    string jsonLogs = commCommand.Args[0];
                    List<MessageRecievedEventArgs> tmpList = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(jsonLogs);
                    foreach (MessageRecievedEventArgs entry in tmpList)
                    {
                        try
                        {
                            //moving the action to be handled in the UI thread
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                this.Logs.Insert(0, entry);
                            });
                        }
                        catch (Exception exc)
                        {
                            string msg = exc.Message;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
