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
    /// interface for the model part of the settings MVVM 
    /// </summary>
    public interface ISettingsModel: INotifyPropertyChanged
    {
        /// <summary>
        /// notify of a property change
        /// </summary>
        /// <param name="propertyName">the changed property</param>
        void NotifyPropertyChanged(string propertyName);
        string OutputDirectory { set; get; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { set; get; }

        /// <summary>
        /// collection od the directoryHandlers directory paths
        /// </summary>
        ObservableCollection<string> DirectoryHandlerPaths { set; get; }

        /// <summary>
        /// method for requesting the config values
        /// </summary>
        void GetConfig();
        /// <summary>
        /// method to send request of removing a directory handler based on a directory path
        /// </summary>
        /// <param name="path">the path of the directory that the handler listens to</param>
        void SendRequestRemoveDirectoryPath(string path);
    }
}
