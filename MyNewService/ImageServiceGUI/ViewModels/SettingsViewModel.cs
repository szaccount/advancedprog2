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
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel model;
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

        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);// to it without saving the info in the vm !!!!!!!!!!!!!!!!!!!!!!!
                };
            this.SubRemove = new DelegateCommand<object>(this.OnSubmit, this.CanSubmit);
            this.PropertyChanged += RemoveCommand;

            /*VM_OutputDirectory = "OUTPUT DIRECTORY";
            VM_SourceName = "source";
            VM_LogName = "name";
            VM_ThumbnailSize = "120";
            VM_DirectoryHandlerPsths = new List<string>();
            VM_DirectoryHandlerPsths.Add("hello.com");
            VM_DirectoryHandlerPsths.Add("otherthing"); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/

        }

        /// <summary>
        /// The function rasing the execute only when it can be execute !!!!!!!!!!!!! change a bit !!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveCommand(object sender, PropertyChangedEventArgs e)
        {
            var command = SubRemove as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// The function do the removing from the list after notifying the tcp client // change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="obj"></param>
        private void OnSubmit(object obj)
        {
            this.model.SendRequestRemoveDirectoryPath(VM_SelectedDirectoryPath);
            this.VM_SelectedDirectoryPath = null;
        }

        /// <summary>
        /// The fucntion checks if the button can be clicked
        /// </summary>
        /// <param name="obj"></param> // change !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// <returns> true for selected item, otherwise false </returns>
        private bool CanSubmit(object obj)
        {
            return !(string.IsNullOrEmpty(this.VM_SelectedDirectoryPath));
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
