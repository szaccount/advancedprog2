using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ImageService.Communication
{
    /// <summary>
    /// class used only as carrier for messages and data between server and clients
    /// </summary>
    public class ServerClientCommunicationCommand
    {

        private CommandEnum commId;
        private string[] args;

        public CommandEnum CommId {
            set { commId = value; }
            get { return commId; }
        }

        public string[] Args {
            set { args = value; }
            get { return args; }
        }

        public ServerClientCommunicationCommand(CommandEnum commandId, string[] args)
        {
            this.commId = commandId;
            this.args = args;
        }

        //empty constructor for the from json method
        private ServerClientCommunicationCommand()
        {

        }

        /// <summary>
        /// converting the instance to json string
        /// </summary>
        /// <returns>the json representation of the instance</returns>
        public string ToJson()
        {
            
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// returns an instance from a json representation of an instance of the class
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static ServerClientCommunicationCommand FromJson(string jsonString)
        {
            if (jsonString != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<ServerClientCommunicationCommand>(jsonString);
                }
                catch (Exception exc)
                {
                    string err = exc.Message;
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}
