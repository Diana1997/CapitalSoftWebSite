using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class TeamMember
    {
        public int TeamMemberID { set; get; }
        [Required]
        public string Firstname { set; get; }
        [Required]
        public string Lastname { set; get; }
        [Required]
        public string Position { set; get; }
        [Required]
        public int?  LanguageID { set; get; }
        public Language Language { set; get; }
        public int? ImageId { set; get; }
        public Image Image { set; get; }
    }
}