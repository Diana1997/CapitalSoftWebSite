using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Technology
    {
        public int TechnologyID { set; get; }
        [Required]
        public string Name { set; get; }
        public virtual ICollection<Project> Projects { set; get; }
        public Technology()
        {
            this.Projects = new HashSet<Project>();
        }
    }
}