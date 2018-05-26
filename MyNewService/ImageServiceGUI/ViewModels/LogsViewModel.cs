using ImageService.Logging.Modal;
using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MessageRecievedEventArgs> VM_Logs
        {
            get
            {
                return this.model.Logs;
            }
        }

        private LogsModel model;

        public LogsViewModel()
        {
            this.model = new LogsModel();
            this.model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };

            /*VM_Logs = new List<MessageRecievedEventArgs>();
            VM_Logs.Add(new MessageRecievedEventArgs { Status = MessageTypeEnum.INFO, Message = "hellofdnjsdjfn"});
            VM_Logs.Add(new MessageRecievedEventArgs { Status = MessageTypeEnum.WARNING, Message = "jbdnbfjkdsfn" }); !!!!!!!!!!!!!!!!!!!!*/

        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
