using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    /// <summary>
    /// interfce for the model of the Logs MVVM
    /// </summary>
    public interface ILogsModel: INotifyPropertyChanged
    {
        /// <summary>
        /// notify of a property change
        /// </summary>
        /// <param name="propertyName">the name of the property</param>
        void NotifyPropertyChanged(string propertyName);
        /// <summary>
        /// collection of log objects
        /// </summary>
        ObservableCollection<MessageRecievedEventArgs> Logs { set; get; }
        /// <summary>
        /// method to request to get the logs
        /// </summary>
        void GetLogs();
    }
}
