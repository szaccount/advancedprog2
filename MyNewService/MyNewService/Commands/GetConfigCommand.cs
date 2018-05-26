using ImageService.Controller.Handlers;
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
        private IDirectoryHandlersManager directoryHandlersManager;

        public GetConfigCommand(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }

        public string Execute(string[] args, out bool result)
        {
            //variable for reading from the configuration file
            var appSettings = ConfigurationManager.AppSettings;

            JObject appConfigJson = new JObject();
            appConfigJson["OutputDir"] = new JValue(appSettings["OutputDir"]);
            appConfigJson["SourceName"] = new JValue(appSettings["SourceName"]);
            appConfigJson["LogName"] = new JValue(appSettings["LogName"]);
            appConfigJson["ThumbnailSize"] = new JValue(appSettings["ThumbnailSize"]);
            //string[] dirPaths = appSettings["Handler"]?.Split(';'); !!!!!!!!!!!!!!!!!!!!!!!!!!!!
            List<string> dirPathsToManage = /*null !!!!!!!!!!!!!!!!!!!!!!!!*/this.directoryHandlersManager.GetDirectoryHandlersPaths();
            /*if (dirPaths != null)
            {
                dirPathsToManage = new List<string>(dirPaths); !!!!!!!!!!!!!!!!!!!!!!
            }*/
            string dirPathsToManageJson = JsonConvert.SerializeObject(dirPathsToManage, Formatting.Indented);
            appConfigJson["dirPathsToManageListString"] = new JValue(dirPathsToManageJson);
            result = true;
            return appConfigJson.ToString();
        }

        public void SetDirectoryHandlersManager(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }
    }
}
