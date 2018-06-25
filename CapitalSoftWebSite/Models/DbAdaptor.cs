using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CapitalSoftWebSite.Models
{
    public class DbAdaptor
    {
        #region Team Members
        public int CreateTeamMember(TeamMember teamMember)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.TeamMembers.Add(teamMember);
                db.SaveChanges();
                return teamMember.TeamMemberID;
            }
        }
        public IList<TeamMember> GetTeamMember()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.TeamMembers.Include(t => t.Image).ToList();
        }
        #endregion


        #region Image
        public int CreateImage(Image image)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Images.Add(image);
                db.SaveChanges();
                return image.ImageID;
            }
        }
        public Image GetImage(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Images.Find(id);
        }
        #endregion

        #region  Technology
        public int CreateTechnology(Technology technology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Technologies.Add(technology);
                db.SaveChanges();
                return technology.TechnologyID;
            }
        }
        public void EditTechnology(Technology technology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(technology).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void DeleteTechnology(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = db.Technologies.Find(id);
                db.Technologies.Remove(technology);
                db.SaveChanges();
            }
        }
        public IList<Technology> GetTechnologies()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Technologies.ToList();
        }

        public Technology GetTechnology(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Technologies.Find(id);
        }

        #endregion

        #region Project
        public int CreateProject(Project project)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return project.ProjectID;
            }
        }
        public IList<Project> GetProjects()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Projects.Include(x=> x.Images).ToList();
        }

        //public IList<Project> GetProjectsHome()
        //{
        //    using (var db = new AppDbContext(ConnectionParameters.connectionString))
        //    {
        //        IList<Project> projectList = new List<Project>();
        //        foreach(var elem in db.Projects)
        //        {
        //            elem.ProjectTechnologies = db.ProjectTechnologies.Where(x => x.P)
        //        }
        //    }
               
        //}
        public Project GetProject(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Projects.Find(id);
        }

        public void DeleteProject(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Project project = db.Projects
                    .Include(x => x.Images)
                    .Where(x => x.ProjectID == id)
                    .ToList()
                    .FirstOrDefault();
                db.Images.RemoveRange(project.Images);
                db.Projects.Remove(project);
                db.SaveChanges();
            }
        }
        #endregion

    }
}