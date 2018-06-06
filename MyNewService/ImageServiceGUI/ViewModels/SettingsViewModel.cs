using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageServiceGUI.Models;
using Microsoft.Practices.Prism.Commands;

namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// view model for the settings MVVM
    /// </summary>
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel model;
        private string selectedDirectoryPath;
        public ICommand SubRemove { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string VM_OutputDirectory
        {
            get
            {
                return this.model.OutputDirectory;
            }
        }
        public string VM_SourceName
        {
            get
            {
                return this.model.SourceName;
            }
        }
        public string VM_LogName
        {
            get
            {
                return this.model.LogName;
            }
        }
        public string VM_ThumbnailSize
        {
            get
            {
                return this.model.ThumbnailSize;
            }
        }
        public string VM_SelectedDirectoryPath
        {
            get
            {
                return this.selectedDirectoryPath;
            }
            set
            {
                if (this.selectedDirectoryPath != value)
                {
                    this.selectedDirectoryPath = value;
                    NotifyPropertyChanged("VM_SelectedDirectoryPath");
                }
            }
        }
        public ObservableCollection<string> VM_DirectoryHandlerPaths
        {
            get
            {
                return this.model.DirectoryHandlerPaths;
            }
        }

        /// <summary>
        /// default constructor for the class
        /// </summary>
        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            this.SubRemove = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            this.PropertyChanged += RemoveCommand;

        }

        /// <summary>
        /// The function rasing the execute only when it can be executed
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">parameters matching for the request</param>
        private void RemoveCommand(object sender, PropertyChangedEventArgs e)
        {
            var command = SubRemove as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// The function for removing the object from the list
        /// </summary>
        /// <param name="obj">the object to remove</param>
        private void OnRemove(object obj)
        {
            this.model.SendRequestRemoveDirectoryPath(VM_SelectedDirectoryPath);
            this.VM_SelectedDirectoryPath = null;
        }

        /// <summary>
        /// The fucntion for checking if the object can be removed and therfore if the button can be clicked
        /// </summary>
        /// <param name="obj">the object to remove</param>
        /// <returns> true if there is a selected object, otherwise returns false </returns>
        private bool CanRemove(object obj)
        {
            return !(string.IsNullOrEmpty(this.VM_SelectedDirectoryPath));
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
