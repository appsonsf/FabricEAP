using GroupFile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupFile
{
    public class ServiceDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
    {
        public ServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
            //optionsBuilder.UseSqlServer("name=GFSfApp_GroupFileDb");
            optionsBuilder.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=EapSfApp_GroupFileDb;Integrated Security=True;MultipleActiveResultSets=true");

            return new ServiceDbContext(optionsBuilder.Options);
        }
    }
}
