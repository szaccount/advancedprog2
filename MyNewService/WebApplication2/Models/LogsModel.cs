using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace WebApplication2.Models
{
    public class LogsModel
    {

        public IEnumerable<LogMessage> GetLogs(string filterBy)
        {
            SynchTcpClientHandler commChannel = new SynchTcpClientHandler();

            bool filter = true;
            if (filterBy ==null || filterBy == "")
                filter = false;
            List<LogMessage> logs = new List<LogMessage>();
            Logm("here1");
            filterBy = filterBy?.ToLower();
            Logm("here2");

            ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.LogCommand, null);
            Logm("here3");
            string receivedMessage = commChannel.Send(commCommand.ToJson());
            Logm("here4");
            ServerClientCommunicationCommand logsCommCommand = ServerClientCommunicationCommand.FromJson(receivedMessage);
            Logm("here5");
            if (logsCommCommand.CommId == CommandEnum.LogCommand)
            {
                Logm("here6");

                string jsonLogs = logsCommCommand.Args[0];
                Logm("here7");
                List<MessageRecievedEventArgs> tmpList = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(jsonLogs);
                Logm("here8");
                foreach (MessageRecievedEventArgs entry in tmpList)
                {
                    if(!filter)
                        logs.Add(new LogMessage() { Message = entry.Message, Status = entry.Status });
                    else 
                        if (entry.Status.ToString().ToLower() == filterBy)
                            logs.Add(new LogMessage() { Message = entry.Message, Status = entry.Status });
                }
                Logm("here9");
                return logs;
            }
            else
            {
                Logm("here bad");
                return new List<LogMessage>();
            }

        }

        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\seanz\Desktop\msglog.txt", msg + Environment.NewLine);
        }
    }
}