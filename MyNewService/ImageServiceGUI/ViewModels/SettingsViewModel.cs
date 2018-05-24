using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Models;

namespace ImageServiceGUI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public string VM_OutputDirectory { get; set; }
        public string VM_SourceName { get; set; }
        public string VM_LogName { get; set; }
        public string VM_ThumbnailSize { get; set; }
        public List<string> VM_DirectoryHandlerPsths { get; set; }

        private SettingsModel model;

        public SettingsViewModel()
        {
            this.model = new SettingsModel();
            this.model.PropertyChanged +=
                delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);// to it without saving the info in the vm !!!!!!!!!!!!!!!!!!!!!!!
                };

            VM_OutputDirectory = "OUTPUT DIRECTORY";
            VM_SourceName = "source";
            VM_LogName = "name";
            VM_ThumbnailSize = "120";
            VM_DirectoryHandlerPsths = new List<string>();
            VM_DirectoryHandlerPsths.Add("hello.com");
            VM_DirectoryHandlerPsths.Add("otherthing");

        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
