using CapitalSoftWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.ViewModel
{
    public class HomePageModel
    {
        public Contact Contact { set; get; }
        public IList<TeamMember> TeamMembers { set; get; }
        public IList<Project> Projects { set; get; }
    }
}