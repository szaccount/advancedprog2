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
    /// <summary>
    /// view model for the logs MVVM, implemnting INotifyPropertyChanged interface
    /// </summary>
    public class LogsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// property of ObservableCollection with get and set func for the log objects
        /// those are the view model logs
        /// </summary>
        public ObservableCollection<MessageRecievedEventArgs> VM_Logs
        {
            get
            {
                return this.model.Logs;
            }
        }

        private ILogsModel model;

        /// <summary>
        /// default c'tor of the class adding a delegate of PropertyChangedEventArgs.
        /// </summary>
        public LogsViewModel()
        {
            this.model = new LogsModel();
            this.model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        /// <summary>
        /// notify of a property change
        /// </summary>
        /// <param name="propertyName">the name of the property</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
