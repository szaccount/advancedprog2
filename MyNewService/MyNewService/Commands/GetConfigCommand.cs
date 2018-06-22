using ImageService.Controller.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.IDirecoryHandlersManager;


namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        private IDirectoryHandlersManager directoryHandlersManager;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="manager">drectoryHandlers manager</param>
        public GetConfigCommand(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }

        /// <summary>
        /// executing the command
        /// </summary>
        /// <param name="args">command arguments</param>
        /// <param name="result">indicating if action successful</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            //variable for reading from the configuration file
            var appSettings = ConfigurationManager.AppSettings;

            JObject appConfigJson = new JObject();
            appConfigJson["OutputDir"] = new JValue(appSettings["OutputDir"]);
            appConfigJson["SourceName"] = new JValue(appSettings["SourceName"]);
            appConfigJson["LogName"] = new JValue(appSettings["LogName"]);
            appConfigJson["ThumbnailSize"] = new JValue(appSettings["ThumbnailSize"]);
            List<string> dirPathsToManage = this.directoryHandlersManager.GetDirectoryHandlersPaths();
            
            string dirPathsToManageJson = JsonConvert.SerializeObject(dirPathsToManage, Formatting.Indented);
            appConfigJson["dirPathsToManageListString"] = new JValue(dirPathsToManageJson);
            result = true;
            return appConfigJson.ToString();
        }

        /// <summary>
        /// method for setting the directoryHandlers manager
        /// </summary>
        /// <param name="manager">the directoryHandlers manager</param>
        public void SetDirectoryHandlersManager(IDirectoryHandlersManager manager)
        {
            this.directoryHandlersManager = manager;
        }
    }
}
