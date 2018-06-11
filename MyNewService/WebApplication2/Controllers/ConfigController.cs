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
        private ConfigModel model;

        public ConfigController()
        {
            model = new ConfigModel();
        }
        
        // GET: Config
        public ActionResult Index()
        {
            return View(model.GetConfig());
        }

        public ActionResult MessageRemoveHandler(string path)
        {
            return View(new PathData() { Path = path});
        }

        public bool RemoveHandler(string path)
        {
            this.model.DeleteHandler(path);
            return true;
        }

    }
}