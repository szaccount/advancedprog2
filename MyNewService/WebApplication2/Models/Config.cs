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
    public class Config
    {
        private TcpClientChannel commChannel;
        ServerClientCommunicationCommand commConfigCommand;

        public Config()
        {
            string[] args = new string[1];
            this.commChannel = TcpClientChannel.GetInstance();
            this.commConfigCommand = new ServerClientCommunicationCommand(CommandEnum.GetConfigCommand, args);
        }


        public void delete(string path)
        {
            string[] args = new string[1];
            args[0] = path;
            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.CloseCommand, args);
            this.commChannel.Write(commCommand.ToJson());
        }

    }
}