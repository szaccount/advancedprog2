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
    public class ConfigController : Controller
    {
        private static string handlerToDelete = "";

        private ConfigModel model = new ConfigModel();
        
        // GET: Config
        public ActionResult Index()
        {
            return View(model.GetConfig());
        }

        public ActionResult MessageRemoveHandler(string path)
        {
            handlerToDelete = path;
            return View(new PathData() { Path = path});
        }

        public ActionResult RemoveHandler()
        {
            this.model.DeleteHandler(handlerToDelete);
            return RedirectToAction("Index");
        }

    }
}