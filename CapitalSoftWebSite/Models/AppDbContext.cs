using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CapitalSoftWebSite.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(string conn) : base(conn) { }

        public DbSet<Image> Images { set; get; }
        public DbSet<Project> Projects { set; get; }
        public DbSet<TeamMember> TeamMembers { set; get; }
        public DbSet<Technology> Technologies { set; get; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<ProjectTechnology> ProjectTechnologies { set; get; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Project>()
        //        .HasMany<Technology>(t => t.Technologies)
        //        .WithMany(p => p.Projects)
        //        .Map(pt =>
        //        {
        //            pt.MapLeftKey("ProjectID");
        //            pt.MapRightKey("TechnologyID");
        //            pt.ToTable("ProjectTechnology");
        //        }
        //   );
        //}
    }
}