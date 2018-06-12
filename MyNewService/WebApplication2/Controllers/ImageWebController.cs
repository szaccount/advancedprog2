using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Newtonsoft.Json.Linq;
using ImageServiceGUI;
using System.Web.Configuration;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    /// <summary>
    ///  The controller for the ImageWeb based web pages
    /// </summary>
    public class ImageWebController : Controller
    {

        static ImageWebModel model = new ImageWebModel();
        static ConfigModel configModel = new ConfigModel();

        // GET: ImageService
        /// <summary>
        /// fanction return an object with the status, number of Image in directory , and students name.
        /// </summary>
        /// <returns>view of the main page of the controller</returns>
        public ActionResult Index()
        {
            ConfigData config = configModel.GetConfig();
            ImageWebData data = model.GetImageWebData(config.OutputDirectory);
            return View(data);
        }

    }
}