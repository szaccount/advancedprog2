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
        private Config model;
        // GET: Config
        public ActionResult Index()
        {
            return View(model);
        }

        private ActionResult removePathFromLidt(string path)
        {
            this.model.delete(path);
            return RedirectToAction("Config");
        }
    }
}