using ImageService.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
namespace WebApplication2.Models
{
    /// <summary>
    /// the model for the ImageWeb controller
    /// </summary>
    public class ImageWebModel
    {
        /// <summary>
        /// method returns the ImageWeb data
        /// </summary>
        /// <param name="pathToGallery">path to the gallery</param>
        /// <returns>ImageWeb data</returns>
        public ImageWebData GetImageWebData(string pathToGallery)
        {
            SynchTcpClientHandler commChannel = new SynchTcpClientHandler();
            ImageWebData webData;
            bool isConnected = commChannel.IsConnected;
            int numberOfPics;
            if (pathToGallery != null && pathToGallery != "")
            {
                try
                {
                    numberOfPics = Directory.GetFiles(pathToGallery, "*.*", SearchOption.AllDirectories).Length;
                }
                catch
                {
                    numberOfPics = 0;
                }
            }
            else
            {
                numberOfPics = 0;
            }
            string[] nameStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Names.txt"));
            string[] idStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Ids.txt"));
            List<string> names = nameStrings.ToList<String>();
            List<string> ids = idStrings.ToList<String>();
            webData = new ImageWebData { IsConnected = isConnected, NumberOfPics = numberOfPics, Names = names, IDs = ids };

            commChannel.Close();

            return webData;
        }
    }
}