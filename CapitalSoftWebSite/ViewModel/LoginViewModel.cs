using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Login { set; get; }
        [Required]
        public string Password { set; get; }
    }
}