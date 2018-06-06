using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrustracture.ToFile
{
    public class LoggerToFile
    {
        /// <summary>
        /// method for debugging purposes. Adjust the path for the txt file to be written to 
        /// </summary>
        /// <param name="msg">msg to be written</param>
        public static void Logm(string msg)
        {
            //File.AppendAllText(@"D:\Users\seanz\Desktop\log1.txt", msg + Environment.NewLine);
        }
    }
}
