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

namespace WebApplication2.Controllers
{
    public class ImageServiceController : Controller
    {
        
        // GET: ImageService
        public ActionResult Index()
        {
            JObject data = GetStatus();
            return View(data);
        }

        /// <summary>
        /// fanction return an JSON object with the status, number of Image in directory , and students name.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private JObject GetStatus()
        { 
            var appSettings = WebConfigurationManager.AppSettings;
            TcpClientChannel commChannel = TcpClientChannel.GetInstance();
            JObject data = new JObject();
            data["status"] = new JValue(commChannel.IsConnected);
            data["counterImage"] = new JValue(Directory.GetFiles(@"/outputDir/", "*.*", SearchOption.AllDirectories).Length);
            data["StudentName"] = new JValue(appSettings["StudentName"]);
            data["ID"] = new JValue(appSettings["ID"]);
            return data;
        }


    }
}