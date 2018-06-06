using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// view model for the main window
    /// </summary>
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TcpClientChannel commChannel;

        //stores the connection status of the system
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

        /// <summary>
        /// constructor for the class
        /// </summary>
        public MainWindowViewModel()
        {
            this.commChannel = TcpClientChannel.GetInstance();
            IsConnected = this.commChannel.IsConnected;
        }
        /// <summary>
        /// notifying of a property change
        /// </summary>
        /// <param name="propertyName">the name of the property</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
