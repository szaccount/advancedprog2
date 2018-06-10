﻿using System;
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
    public class ImageWebController : Controller
    {

        static ImageWebModel model = new ImageWebModel();

        // GET: ImageService
        /// <summary>
        /// fanction return an object with the status, number of Image in directory , and students name.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ImageWebData data = model.GetImageWebData();
            return View(data);
        }

    }
}