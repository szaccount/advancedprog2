using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class LogsModel
    {

        public IEnumerable<LogMessage> GetLogs(string filterBy)
        {
            filterBy = filterBy?.ToLower();
            if (filterBy == "")
                return new List<LogMessage>()
            {
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.INFO, Message = "info log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.WARNING, Message = "warning log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.FAIL, Message = "fail log message"}
            };
            else
                if (filterBy == "info")
                return new List<LogMessage>() { new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.INFO, Message = "yey" } };
            return new List<LogMessage>()
            {
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.INFO, Message = "info log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.WARNING, Message = "warning log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.FAIL, Message = "fail log message"}
            };
        }
    }
}