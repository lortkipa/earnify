using Data.Configurations;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ProjectContext : DbContext
    {
        // DBSet properties
        public DbSet<User> Users { get; set; }

        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> context) : base(context)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // seeding
        }
    }
}
