namespace CapitalSoftWebSite.Models
{
    public class ProjectTechnology
    {
        public int? ProjectID { set; get; }
        public Project Project { set; get; }
        public int? TechnologyID { set; get; }
        public Technology Technology { set; get; }
    }
}