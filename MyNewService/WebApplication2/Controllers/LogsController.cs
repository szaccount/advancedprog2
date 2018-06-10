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

        static LogsModel model = new LogsModel();

        public ActionResult Index(string statusToFilter)
        {
            return View(model.GetLogs(statusToFilter));
        }

    }
}