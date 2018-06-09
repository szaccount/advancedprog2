using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LogsController : Controller
    {
        
        // GET: Photo
        public ActionResult Index()
        {
            IEnumerable<LogMessage> logs;//receive the list from a model that handles logs logic against the service !!!!!!!!!!!!!!!!!!!!!!
            logs = new List<LogMessage>()
            {
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.INFO, Message = "info log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.WARNING, Message = "warning log message"},
                new LogMessage { Status = ImageService.Logging.Modal.MessageTypeEnum.FAIL, Message = "fail log message"}
            };
            return View(logs);
        }

    }
}