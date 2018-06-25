using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Project
    {
        public int ProjectID { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        [MaxLength(4000)]
        public string Description { set; get; }
        public ICollection<Image> Images { set; get; }
        public string Lang { set; get; }
        public ICollection<ProjectTechnology> ProjectTechnologies { set; get; } 
    }
}