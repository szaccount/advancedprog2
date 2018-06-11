using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class ConfigData
    {
        [Required]
        [Display(Name = "OutputDirectory")]
        [DataType(DataType.Text)]
        public string OutputDirectory { set; get; }
        [Required]
        [Display(Name = "SourceName")]
        [DataType(DataType.Text)]
        public string SourceName { get; set; }
        [Required]
        [Display(Name = "LogName")]
        [DataType(DataType.Text)]
        public string LogName { get; set; }
        [Required]
        [Display(Name = "ThumbnailSize")]
        [DataType(DataType.Text)]
        public string ThumbnailSize { set; get; }
        [Required]
        [Display(Name = "Message")]
        public List<string> DirectoryHandlerPaths { set; get; }
    }
}