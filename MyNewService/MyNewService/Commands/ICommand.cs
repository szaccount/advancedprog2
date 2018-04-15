using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// interface specifying the properties of command class
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// method for executing the command
        /// </summary>
        /// <param name="args">command's arguments</param>
        /// <param name="result">the result of command execution</param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);
    }
}
