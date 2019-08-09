using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GroupFile.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UnitTestCommon;

namespace GroupFileServiceTest
{
    public class TheGroupFileAppServiceTestBase : AppServiceTestBase
    {
        protected static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GroupFile.MappingProfile>();
            });
            return config.CreateMapper();
        }

        protected static (SqliteConnection, DbContextOptions<ServiceDbContext>) OpenDb()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                //.UseLazyLoadingProxies()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(TestLoggerFactory)
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            using (var context = new ServiceDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            return (connection, options);
        }
    }
}
