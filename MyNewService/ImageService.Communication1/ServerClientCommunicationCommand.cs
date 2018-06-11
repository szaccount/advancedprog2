using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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

        public string ToJson()
        {
            JObject sccc = new JObject();
            sccc["CommId"] = new JValue(commId);
            sccc["Args"] = new JValue(Args);

            return sccc.ToString();
        }

        public static ServerClientCommunicationCommand FromJson(string jsonString)
        {
            ServerClientCommunicationCommand scccObject = new ServerClientCommunicationCommand();
            try
            {
                JObject sccc = JObject.Parse(jsonString);
                scccObject.CommId = (CommandEnum)Enum.Parse(typeof(CommandEnum), (string)sccc["CommId"]);
                JArray jArr = (JArray)sccc["Args"];
                scccObject.Args = jArr.ToObject<string[]>();
                return scccObject;
            }
            catch (Exception exc)
            {
                throw new ArgumentException("invalid string received");
            }
        }
    }
}
