using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TcpClientChannel commChannel;

        private bool isConnected;
        
        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
            set
            {
                if (this.isConnected != value)
                {
                    this.isConnected = value;
                    NotifyPropertyChanged("IsConnected");
                }
            }
        }

        public MainWindowViewModel()
        {
            this.commChannel = TcpClientChannel.GetInstance();
            IsConnected = this.commChannel.IsConnected;
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
