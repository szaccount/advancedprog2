using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    ///  The controller for the logs based web pages
    /// </summary>
    public class LogsController : Controller
    {

        static LogsModel model = new LogsModel();

        /// <summary>
        /// the main page view method
        /// </summary>
        /// <param name="statusToFilter">the requested status string to filter by</param>
        /// <returns>updated main page view</returns>
        public ActionResult Index(string statusToFilter)
        {
            return View(model.GetLogs(statusToFilter));
        }

    }
}