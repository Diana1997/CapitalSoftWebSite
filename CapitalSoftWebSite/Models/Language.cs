using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class Language
    {
        public int LanguageID { set; get; }
        public string Lang { set; get; }

        public ICollection<TeamMember> TeamMembers { set; get; }
        public Language ()
        {
            TeamMembers = new List<TeamMember>();
        }
    }
}