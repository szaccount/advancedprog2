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
    public class ImageWebModel
    {
        public ImageWebData GetImageWebData()
        {
            SynchTcpClientHandler commChannel = new SynchTcpClientHandler();
            ImageWebData webData;
            bool isConnected = commChannel.IsConnected;

            int numberOfPics = Directory.GetFiles(HttpContext.Current.Server.MapPath("/PhotosDirectories/Gallery"), "*.*", SearchOption.AllDirectories).Length;
            string[] nameStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Names.txt"));
            string[] idStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Ids.txt"));
            List<string> names = nameStrings.ToList<String>();
            List<string> ids = idStrings.ToList<String>();
            webData = new ImageWebData { IsConnected = isConnected, NumberOfPics = numberOfPics, Names = names, IDs = ids };

            commChannel.Close();

            return webData;
        }

        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\seanz\Desktop\msglog.txt", msg + Environment.NewLine);
        }
    }
}