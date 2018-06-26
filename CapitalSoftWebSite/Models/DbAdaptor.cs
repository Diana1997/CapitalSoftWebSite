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
        public IList<TeamMember> GetTeamMembers()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.TeamMembers.Include(t => t.Image).ToList();
        }
        public TeamMember GetTeamMember(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.TeamMembers.Include(x => x.Image).Where(x => x.TeamMemberID == id).FirstOrDefault();
        }

        public void DeleteTeamMember(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                TeamMember teamMember = db.TeamMembers
                    .Include(x => x.Image)
                    .Where(x => x.TeamMemberID == id)
                    .FirstOrDefault();
                if (teamMember.Image != null)
                    db.Images.Remove(teamMember.Image);
                db.TeamMembers.Remove(teamMember);
                db.SaveChanges();
            }
               
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

        public IList<Project> GetProjectsHome()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                var projectList = db.Projects.Include(x => x.Images).ToList();
                foreach (var elem in projectList)
                {
                    var ptList = db.ProjectTechnologies.Where(x => x.ProjectID == elem.ProjectID).ToList();
                    foreach(var k in ptList)
                    {
                        var technolgy = db.Technologies.Where(x => x.TechnologyID == k.TechnologyID).FirstOrDefault();
                        if(technolgy != null)
                            elem.Technologies.Add(technolgy);
                    }
                }
                return projectList;
            }
            
        }
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

        #region ProjectTechnology
        public int CreateProjectTechnology(ProjectTechnology projectTechnology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.ProjectTechnologies.Add(projectTechnology);
                db.SaveChanges();
                return projectTechnology.ProjectTechnologyID;
            }
        }
        #endregion

    }
}