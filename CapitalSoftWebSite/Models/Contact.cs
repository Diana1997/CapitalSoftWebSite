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
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "Name_Required")]
        public string  Name { set; get; }
        [Required]
        public  string Email { set; get; }
        public string Phone { set; get; }
        [Required]
        [Display(ResourceType = typeof(Resources.Resource), Name = nameof(Resources.Resource.Message))]
        public string Message { set; get; }
    }
}