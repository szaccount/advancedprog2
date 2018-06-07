using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Config
        public ActionResult Index()
        {
            return View();
        }
        
        /*private void InitializeConfigData() {
            JObject appConfigData = JObject.Parse(jsonData);
            OutputDirectory = (string)appConfigData["OutputDir"];
            SourceName = (string)appConfigData["SourceName"];
            LogName = (string)appConfigData["LogName"];
            ThumbnailSize = (string)appConfigData["ThumbnailSize"];
            string dirPathsListString = (string)appConfigData["dirPathsToManageListString"];
            DirectoryHandlerPaths = new ObservableCollection<string>(JsonConvert.DeserializeObject<List<string>>(dirPathsListString));

        }*/
    }
}