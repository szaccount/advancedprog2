using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApplication2.Models
{
    public class ImageWebModel
    {
        public ImageWebData GetImageWebData()
        {
            //TcpClientChannel commChannel = TcpClientChannel.GetInstance();
            ImageWebData webData;
            var appSettings = WebConfigurationManager.AppSettings;
            bool isConnected = /*commChannel.IsConnected*/true;

            int numberOfPics = Directory.GetFiles(HttpContext.Current.Server.MapPath("/PhotosDirectories/Gallery"), "*.*", SearchOption.AllDirectories).Length;
            Logm(numberOfPics.ToString());
            string[] nameStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Names.txt"));
            Logm(nameStrings[0]);
            string[] idStrings = File.ReadAllLines(HttpContext.Current.Server.MapPath("/App_Data/Ids.txt"));
            Logm(idStrings[0]);
            List<string> names = nameStrings.ToList<String>();
            List<string> ids = idStrings.ToList<String>();
            webData = new ImageWebData { IsConnected = isConnected, NumberOfPics = numberOfPics, Names = names, IDs = ids };
            return webData;
        }

        private static void Logm(string msg)
        {
            File.AppendAllText(@"D:\Users\seanz\Desktop\msglog.txt", msg + Environment.NewLine);
        }
    }
}