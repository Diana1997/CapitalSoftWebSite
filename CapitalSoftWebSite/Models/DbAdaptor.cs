using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CapitalSoftWebSite.Models
{
    public class DbAdaptor
    {
        #region Team Members
        public static async Task<int> CreateTeamMemberAsync(TeamMember teamMember)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.TeamMembers.Add(teamMember);
                await db.SaveChangesAsync();
                return teamMember.TeamMemberID;
            }
        }
        public static async Task<IList<TeamMember>> GetTeamMembersAsync()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.TeamMembers.Include(t => t.Image)?.ToListAsync();
        }

        public static async Task<IList<TeamMember>> GetTeamMembersAsync(string culture)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.TeamMembers.Include(t => t.Image).Where(x => x.Lang == culture)?.ToListAsync();
        }
        public static async Task<TeamMember> GetTeamMemberAsync(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.TeamMembers.Include(x => x.Image).Where(x => x.TeamMemberID == id).FirstOrDefaultAsync();
        }

        public static async Task DeleteTeamMemberAsync(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                TeamMember teamMember = await db.TeamMembers
                    .Include(x => x.Image)
                    .Where(x => x.TeamMemberID == id)
                    .FirstOrDefaultAsync();
                if (teamMember.Image != null)
                    db.Images.Remove(teamMember.Image);
                db.TeamMembers.Remove(teamMember);
                await db.SaveChangesAsync();
            }
        }
        public static async Task EditTeamMemberAsync(TeamMember teamMember)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(teamMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region Projects
        public static async Task<int> CreateProjectAsync(Project project)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return project.ProjectID;
            }
        }

        public static async Task EditProjectAsync(Project project)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        public static async Task<IList<Project>> GetProjectsAsync()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Projects.Include(x => x.Images).ToListAsync();
        }

        public static async Task<IList<Project>> GetProjectsFullAsync(string culture)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = null;
                var projectList = await db.Projects.Include(x => x.Images).Where(x => x.Lang == culture).ToListAsync();

                foreach (var elem in projectList)
                {
                    var ptList = await db.ProjectTechnologies.Where(x => x.ProjectID == elem.ProjectID).ToListAsync();
                    foreach (var k in ptList)
                    {
                        technology = db.Technologies.Where(x => x.TechnologyID == k.TechnologyID).FirstOrDefault();
                        if (technology != null)
                            elem.Technologies.Add(technology);
                    }
                }
                return projectList;
            }
        }

        public static async Task<Project> GetProjectAsync(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = null;
                Project project = db.Projects.Include(x => x.Images).Where(x => x.ProjectID == id).FirstOrDefault();
                var projectTechnologies = await db.ProjectTechnologies.Where(x => x.ProjectID == project.ProjectID).ToListAsync();
                foreach (var elem in projectTechnologies)
                {
                    technology = db.Technologies.Where(x => x.TechnologyID == elem.TechnologyID).FirstOrDefault();
                    project.Technologies.Add(technology);
                }
                return project;
            }
        }

        public static async Task DeleteProjectAsync(int id)
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
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region Contact
        public static async Task<int> CreateContactAsync(Contact contact)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return contact.ContactID;
            }
        }
        public static async Task<IList<Contact>> GetContactsAsync()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Contacts.ToListAsync();
        }

        public static async Task<Contact> GetContactAsync(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Contacts.FindAsync(id);
        }

        public static async Task DeleteContactAsync(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Contact contact = await db.Contacts.FindAsync(id);
                db.Contacts.Remove(contact);
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region  Technology
        public static async Task<int> CreateTechnologyAsync(Technology technology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Technologies.Add(technology);
                await db.SaveChangesAsync();
                return technology.TechnologyID;
            }
        }
        public static async Task EditTechnologyAsync(Technology technology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Entry(technology).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
        public static async Task DeleteTechnologyAsync(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Technology technology = await db.Technologies.FindAsync(id);
                db.Technologies.Remove(technology);
                await db.SaveChangesAsync();
            }
        }
        public static async Task<IList<Technology>> GetTechnologiesAsync()
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Technologies?.ToListAsync();
        }

        public static async Task<Technology> GetTechnologyAsync(int? id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Technologies.FindAsync(id);
        }

        #endregion

        #region Image
        public static async Task<int> CreateImageAsync(Image image)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.Images.Add(image);
                await db.SaveChangesAsync();
                return image.ImageID;
            }
        }
        public static async Task<Image> GetImageAsync(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
                return await db.Images.FindAsync(id);
        }
        public static async Task DeleteImageAsync(int id)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                Image image = db.Images.Find(id);
                db.Images.Remove(image);
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region ProjectTechnology
        public static async Task<int> CreateProjectTechnologyAsync(ProjectTechnology projectTechnology)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                db.ProjectTechnologies.Add(projectTechnology);
                await db.SaveChangesAsync();
                return projectTechnology.ProjectTechnologyID;
            }
        }
        public static async Task DeleteProjectTechnologyAsync(int projectID, int technologyID)
        {
            using (var db = new AppDbContext(ConnectionParameters.connectionString))
            {
                ProjectTechnology projectTechnology = db.ProjectTechnologies
                    .Where(x => x.ProjectID == projectID && x.TechnologyID == technologyID)
                    .FirstOrDefault();
                db.ProjectTechnologies.Remove(projectTechnology);
                await db.SaveChangesAsync();
            }
        }
        #endregion
    }
}