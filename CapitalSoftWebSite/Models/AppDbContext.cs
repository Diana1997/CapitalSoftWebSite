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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectTechnology>()
                .HasKey(pt => new { pt.ProjectID, pt.TechnologyID });

            modelBuilder.Entity<ProjectTechnology>()
                .HasOptional(pt => pt.Project)
                .WithMany(p => p.ProjectTechnologies)
                .HasForeignKey(pt => pt.ProjectID);

            modelBuilder.Entity<ProjectTechnology>()
              .HasOptional(pt => pt.Technology)
              .WithMany(p => p.ProjectTechnologies)
              .HasForeignKey(pt => pt.TechnologyID);
        }
    }
}