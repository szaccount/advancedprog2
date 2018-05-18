using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            //variable for reading from the configuration file
            var appSettings = ConfigurationManager.AppSettings;

            JObject appConfigJson = new JObject();
            appConfigJson["OutputDir"] = appSettings["OutputDir"];
            appConfigJson["SourceName"] = appSettings["SourceName"];
            appConfigJson["LogName"] = appSettings["LogName"];
            appConfigJson["ThumbnailSize"] = appSettings["ThumbnailSize"];
            string[] dirPaths = appSettings["Handler"]?.Split(';');
            List<string> dirPathsToManage = null;
            if (dirPaths != null)
            {
                dirPathsToManage = new List<string>(dirPaths);
            }
            string dirPathsToManageJson = JsonConvert.SerializeObject(dirPathsToManage, Formatting.Indented);
            appConfigJson["dirPathsToManageListString"] = dirPathsToManageJson;
            result = true;
            return appConfigJson.ToString();
        }
    }
}
