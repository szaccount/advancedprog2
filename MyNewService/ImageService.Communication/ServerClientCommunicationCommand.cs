using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ImageService.Infrustracture.ToFile;
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

        public string ToJson()
        {
            /*LoggerToFile.Logm("In ServerClientCommunicationCommand received string in ToJson, procceeding to handle it 1");
            JObject sccc = new JObject();
            LoggerToFile.Logm("In ServerClientCommunicationCommand received string in ToJson, procceeding to handle it 2");
            sccc["CommId"] = new JValue(commId);
            LoggerToFile.Logm("In ServerClientCommunicationCommand received string in ToJson, procceeding to handle it 3");
            sccc["ArgsString"] = JsonConvert.SerializeObject(Args); //new JValue(Args); !!!!!!!! was !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            LoggerToFile.Logm("In ServerClientCommunicationCommand received string in ToJson, procceeding to handle it 4");

            return sccc.ToString();*/
            LoggerToFile.Logm("In ServerClientCommunicationCommand in ToJson, procceeding to handle it 1, the return is: " + JsonConvert.SerializeObject(this, Formatting.Indented));
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static ServerClientCommunicationCommand FromJson(string jsonString)
        {
            LoggerToFile.Logm("In ServerClientCommunicationCommand received string in FromJson, procceeding to handle it 1");
            if (jsonString != null)
            {
                try
                {
                    LoggerToFile.Logm("In ServerClientCommunicationCommand received string in FromJson, procceeding to handle it 2");
                    return JsonConvert.DeserializeObject<ServerClientCommunicationCommand>(jsonString);
                }
                catch (Exception exc)
                {
                    string err = exc.Message;
                    LoggerToFile.Logm(err); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    return null;
                }
            }
            else
            {
                LoggerToFile.Logm("In ServerClientCommunicationCommand received string in FromJson, procceeding to handle it 3");
                return null;
            }
            /*ServerClientCommunicationCommand scccObject = new ServerClientCommunicationCommand();
            try
            {
                JObject sccc = JObject.Parse(jsonString);
                scccObject.CommId = (CommandEnum)Enum.Parse(typeof(CommandEnum), (string)sccc["CommId"]);
                //JArray jArr = null; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                scccObject.Args = null;
                if (sccc["ArgsString"] != null)
                {
                //    scccObject.Args = (JArray)sccc["ArgsString"].................... JsonConvert.DeserializeObject<string[]>(sccc["ArgsString"]);
                }
                //scccObject.Args = jArr?.ToObject<string[]>(); !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return scccObject;
            }
            catch (Exception exc)
            {
                LoggerToFile.Logm("In ServerClientCommunicationCommand received string in FromJson, procceeding to handle it Exception");
                throw new ArgumentException("invalid string received");
            }*/
        }
    }
}
