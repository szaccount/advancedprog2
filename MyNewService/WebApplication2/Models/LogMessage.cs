using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ImageService.Logging.Modal;

namespace WebApplication2.Models
{
    public class LogMessage
    {
        //the type of message
        [Required]
        [Display(Name = "Status")]
        public MessageTypeEnum Status { get; set; }
        //the message itself
        [Required]
        [Display(Name = "Message")]
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
}