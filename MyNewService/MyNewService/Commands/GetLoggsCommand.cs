using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageService.Commands
{
    //execute of this command returns a list of the loggs in the system in json form
    class GetLoggsCommand : ICommand
    {
        private ILoggsRecorder m_logsRecorder;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logsRecorder">log recording object</param>
        public GetLoggsCommand(ILoggsRecorder logsRecorder)
        {
            this.m_logsRecorder = logsRecorder;
        }

        /// <summary>
        /// executing the command
        /// </summary>
        /// <param name="args"><command arguments/param>
        /// <param name="result">indicating if action was successful</param>
        /// <returns>a list of the loggs in the system in json form</returns>
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
