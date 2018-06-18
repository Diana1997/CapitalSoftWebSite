using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Contact
    {
        public int ContactID { set; get; }
        [Required]
        public string  Name { set; get; }
        [Required]
        public  string Email { set; get; }
        [Required]
        public string Phone { set; get; }
        [Required]
        public string Message { set; get; }
    }
}