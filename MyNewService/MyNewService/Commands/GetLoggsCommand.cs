using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging;
using Logging.Modal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageService.Commands
{
    //execute of this command returns a list of the loggs in the system in json form
    class GetLoggsCommand : ICommand
    {
        private ILoggsRecorder m_logsRecorder;

        public GetLoggsCommand(ILoggsRecorder logsRecorder)
        {
            this.m_logsRecorder = logsRecorder;
        }

        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<MessageRecievedEventArgs> listOfLogs = this.m_logsRecorder.GetLoggsRecord();
                string jsonList = JsonConvert.SerializeObject(listOfLogs, Formatting.Indented);
                result = true;
                return jsonList;
            }
            catch (Exception exc)
            {
                result = false;
                return "";
            }
        }
    }
}
