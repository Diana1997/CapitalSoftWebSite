using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapitalSoftWebSite.Models
{
    public class Image
    {
        public int ImageID { set; get; }
        [Required]
        public byte[] ImageData { set; get; }
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { set; get; }

        public ICollection<TeamMember> TeamMembers { set; get; }

        public int? ProjectID { set; get; }
    }
}