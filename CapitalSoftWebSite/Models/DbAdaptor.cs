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
        public void EditTeamMember(TeamMember teamMember)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(teamMember).State = EntityState.Modified;
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
        public void DeleteImage(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Image image = db.Images.Find(id);
                db.Images.Remove(image);
                db.SaveChanges();
            }
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

        public void EditProject(Project project)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public IList<Project> GetProjects()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Projects.Include(x=> x.Images).ToList();
        }

        public IList<Project> GetProjectsFull()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = null;
                var projectList = db.Projects.Include(x => x.Images).ToList();

                foreach (var elem in projectList)
                {
                    var ptList = db.ProjectTechnologies.Where(x => x.ProjectID == elem.ProjectID).ToList();
                    foreach(var k in ptList)
                    {
                        technology = db.Technologies.Where(x => x.TechnologyID == k.TechnologyID).FirstOrDefault();
                        if (technology != null)
                            elem.Technologies.Add(technology);
                    }
                }
                return projectList;
            }
        }
       
        public Project GetProject(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = null;
                Project project = db.Projects.Include(x => x.Images).Where(x => x.ProjectID == id).FirstOrDefault();
                var projectTechnologies = db.ProjectTechnologies.Where(x => x.ProjectID == project.ProjectID).ToList();
                foreach(var elem in projectTechnologies)
                {
                    technology = db.Technologies.Where(x => x.TechnologyID == elem.TechnologyID).FirstOrDefault();
                    project.Technologies.Add(technology);
                }
                return project;
            }
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
                var ptList = db.ProjectTechnologies.Where(x => x.ProjectID == project.ProjectID);
                db.ProjectTechnologies.RemoveRange(ptList);
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
        public void DeleteProjectTechnology(int projectID, int technologyID)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                ProjectTechnology projectTechnology = db.ProjectTechnologies
                    .Where(x => x.ProjectID == projectID && x.TechnologyID == technologyID)
                    .FirstOrDefault();
                db.ProjectTechnologies.Remove(projectTechnology);
                db.SaveChanges();
            }
        }
        #endregion

        #region Contact
        public int CreateContact(Contact contact)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return contact.ContactID;
            }
        }
        public IList<Contact> GetContacts()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Contacts.ToList();
        }
        
        public Contact GetContact(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return db.Contacts.Find(id);
        }

        public void DeleteContact(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Contact contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
                db.SaveChanges();
            }
        }
        #endregion

    }
}