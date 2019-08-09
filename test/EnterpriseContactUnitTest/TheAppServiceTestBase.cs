using AutoMapper;
using EnterpriseContact;
using InstantMessage;
using AppsOnSF.Common.BaseServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Notification;
using NSubstitute;
using System;
using UnitTestCommon;

namespace EnterpriseContactUnitTest
{
    public class TheAppServiceTestBase : AppServiceTestBase
    {
        protected static IConversationCtrlAppService CreateMockConversationCtrlAppService() 
            => Substitute.For<IConversationCtrlAppService>();

        protected static INotifySessionActor CreateMockNotifySessionActorFactory(Guid userId) 
            => Substitute.For<INotifySessionActor>();

        protected static ISimpleKeyValueService CreateMockSimpleKeyValueService()
           => Substitute.For<ISimpleKeyValueService>();

        protected static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMappingProfile>();
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
