using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    /// <summary>
    /// object that holds the data of imageWeb page
    /// </summary>
    public class ImageWebData
    {
        [Required]
        [Display(Name = "IsConnected")]
        public bool IsConnected { set; get; }
        [Required]
        [Display(Name = "NumberOfPics")]
        public int NumberOfPics { get; set; }
        [Required]
        [Display(Name = "Names")]
        public List<string> Names { get; set; }
        [Required]
        [Display(Name = "IDs")]
        public List<string> IDs { get; set; }

    }
}