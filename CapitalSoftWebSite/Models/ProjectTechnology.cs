using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class ProjectTechnology
    {
        public int ProjectTechnologyID { set; get; }
        public int ProjectID { set; get; }
        public Project Project { set; get; }
        public int TechnologyID { set; get; }
        public Technology Technology { set; get; }
    }
}