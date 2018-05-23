using ImageService.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using ImageService.Infrastructure.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    public class SettingsModel
    {
        private TcpClientChannel commChannel;
        public string OutputDirectory { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<string> DirectoryHandlerPsths {get; set;}

        public SettingsModel()
        {
            commChannel = TcpClientChannel.GetInstance();
        }

        public JObject GetConfig()
        {

        }

        public List<string> GetPathsToDirectoryHandlers()
        {

        }

        private void ReadRecivedMessage(object sender, MessageCommunicationEventArgs messageArgs)
        {
            string message = messageArgs.Message;
            ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
            switch (commCommand.CommId)
            {
                case CommandEnum.GetConfigCommand:
                    break;
                case CommandEnum.CloseCommand:
                    break;
                default:
                    break;
            }
        }

        // delegate command of erasing a directory handlers after choosing path and clicking button !!!!!!!!!!!!!!!!!
    }
}
