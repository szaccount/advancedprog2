using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ImageWebData
    {
        public bool IsConnected { set; get; }
        public int NumberOfPics { get; set; }
        public List<string> Names { get; set; }
        public List<string> IDs { get; set; }

    }
}