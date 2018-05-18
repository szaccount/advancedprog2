using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace ImageService.Infrastructure
{
    /// <summary>
    /// interface specifying the properties of controller class
    /// </summary>
    public interface ICommandExecuter
    {
        /// <summary>
        /// Method for requesting the controller object to execute specific command
        /// </summary>
        /// <param name="commandID">the id of the command</param>
        /// <param name="args">arguments for the command execution request</param>
        /// <param name="result">the result of the execution</param>
        /// <returns>string indicating of success/failure</returns>
        string ExecuteCommand(CommandEnum commandID, string[] args, out bool result);
    }
}
