using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class IClientHandlerCloseEventArgs: EventArgs
    {
        /// <summary>
        /// closing message from the IClientHandler object
        /// </summary>
        public string Message { get; set; }
    }
}
