using ImageService.Infrastructure.Enums;
using ImageService.Communication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageServiceGUI;

namespace WebApplication2.Models
{
    public class ConfigModel
    {
        ServerClientCommunicationCommand commConfigCommand;

        public ConfigData GetConfig()
        {
            try
            {
                ConfigData cData = new ConfigData();
                string[] args = new string[1];
                SynchTcpClientHandler commChannel = new SynchTcpClientHandler();
                this.commConfigCommand = new ServerClientCommunicationCommand(CommandEnum.GetConfigCommand, args);

                string message = commChannel.Send(this.commConfigCommand.ToJson());
                ServerClientCommunicationCommand commCommand = ServerClientCommunicationCommand.FromJson(message);
                if (commCommand.CommId == CommandEnum.GetConfigCommand)
                {
                    string jsonData = commCommand.Args[0];
                    JObject appConfigData = JObject.Parse(jsonData);
                    cData.OutputDirectory = (string)appConfigData["OutputDir"];
                    cData.SourceName = (string)appConfigData["SourceName"];
                    cData.LogName = (string)appConfigData["LogName"];
                    cData.ThumbnailSize = (string)appConfigData["ThumbnailSize"];
                    string dirPathsListString = (string)appConfigData["dirPathsToManageListString"];
                    cData.DirectoryHandlerPaths = new List<string>(JsonConvert.DeserializeObject<List<string>>(dirPathsListString));
                    commChannel.Close();
                    return cData;
                }
                else
                {
                    commChannel.Close();
                    return new ConfigData() { LogName = "", SourceName = "", ThumbnailSize = "", OutputDirectory = "", DirectoryHandlerPaths = new List<string>() };
                }
            }
            catch
            {
                return new ConfigData() { LogName = "", SourceName = "", ThumbnailSize = "", OutputDirectory = "", DirectoryHandlerPaths = new List<string>() };
            }

        }


        public void DeleteHandler(string path)
        {
            SynchTcpClientHandler commChannel = new SynchTcpClientHandler();
            string[] args = new string[1];
            args[0] = path;
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.CloseCommand, args);
            commChannel.Send(commCommand.ToJson());
        }

    }
}