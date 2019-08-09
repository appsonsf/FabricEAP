using GroupFile.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace GroupFile.Models
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options):base(options)
        {
        }
        

        public DbSet<FileItem> FileItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileItem>()
                .HasIndex(o => o.GroupId);
        }
    }
}
