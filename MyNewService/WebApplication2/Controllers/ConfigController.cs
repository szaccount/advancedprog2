using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    /// The controller for the config based web pages
    /// </summary>
    public class ConfigController : Controller
    {
        //the path of the handler in process of deletion
        private static string handlerToDelete = "";

        private ConfigModel model = new ConfigModel();
        
        // GET: Config
        /// <summary>
        /// the main page view method
        /// </summary>
        /// <returns>the main view page of this controller</returns>
        public ActionResult Index()
        {
            return View(model.GetConfig());
        }

        /// <summary>
        /// the view of removing a handler
        /// </summary>
        /// <param name="path">path of the handler</param>
        /// <returns>the appropriate view</returns>
        public ActionResult MessageRemoveHandler(string path)
        {
            handlerToDelete = path;
            return View(new PathData() { Path = path});
        }

        /// <summary>
        /// method for removing a handler, redirects to main page of the controller
        /// </summary>
        /// <returns>view of the main page of the controller</returns>
        public ActionResult RemoveHandler()
        {
            this.model.DeleteHandler(handlerToDelete);
            return RedirectToAction("Index");
        }

    }
}