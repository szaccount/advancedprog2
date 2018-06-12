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
    /// <summary>
    /// model for the logs controller
    /// </summary>
    public class LogsModel
    {

        /// <summary>
        /// method returns the logs from the service
        /// </summary>
        /// <param name="filterBy">string representing status to filter by</param>
        /// <returns>list of filtered logs</returns>
        public IEnumerable<LogMessage> GetLogs(string filterBy)
        {
            try
            {
                SynchTcpClientHandler commChannel = new SynchTcpClientHandler();

                bool filter = true;
                if (filterBy == null || filterBy == "")
                    filter = false;
                List<LogMessage> logs = new List<LogMessage>();
                filterBy = filterBy?.ToLower();

                ServerClientCommunicationCommand commCommand = new ServerClientCommunicationCommand(CommandEnum.LogCommand, null);
                string receivedMessage = commChannel.Send(commCommand.ToJson());
                ServerClientCommunicationCommand logsCommCommand = ServerClientCommunicationCommand.FromJson(receivedMessage);
                if (logsCommCommand.CommId == CommandEnum.LogCommand)
                {
                    string jsonLogs = logsCommCommand.Args[0];
                    List<MessageRecievedEventArgs> tmpList = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(jsonLogs);
                    foreach (MessageRecievedEventArgs entry in tmpList)
                    {
                        if (!filter)
                            logs.Add(new LogMessage() { Message = entry.Message, Status = entry.Status });
                        else
                            if (entry.Status.ToString().ToLower() == filterBy)
                            logs.Add(new LogMessage() { Message = entry.Message, Status = entry.Status });
                    }
                    return logs;
                }
                else
                {
                    return new List<LogMessage>();
                }
            }
            catch
            {
                return new List<LogMessage>();
            }
        }
    }
}