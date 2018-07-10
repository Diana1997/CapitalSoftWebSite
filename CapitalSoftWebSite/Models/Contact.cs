using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;

namespace CapitalSoftWebSite.Models
{
    public class Contact
    {
        public int ContactID { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Name_Required")]
        [RegularExpression(@"(\w+)",
            ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_name")]
        [StringLength(40, ErrorMessageResourceType =typeof(Resource), ErrorMessageResourceName ="Name_Email_Size")]
        public string  Name { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Email_required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_email")]
        [StringLength(40,
            ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Name_Email_Size")]
        public string Email { set; get; }

        [RegularExpression(@"[0-9]+",
            ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_phone_number")]
        [StringLength(25,
            ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Phone_Size")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Message_required")]
        public string Message { set; get; }
    }
}