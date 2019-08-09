using EnterpriseContact.Entities;
using EnterpriseContactService.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseContact
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext()
        {

        }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=EapSfApp_EnterpriseContactDb;Integrated Security=True;MultipleActiveResultSets=true");
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeePosition> EmployeePositions { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<MdmDataHistory> MdmDataHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasIndex(o => o.Number);

            modelBuilder.Entity<Employee>()
               .HasIndex(o => o.Number);
            modelBuilder.Entity<Employee>()
              .HasIndex(o => o.UserId);
            //modelBuilder.Entity<Employee>()
            //  .HasIndex(o => o.UserName);
            //modelBuilder.Entity<Employee>()
            //  .HasIndex(o => o.IdCardNo);
            //modelBuilder.Entity<Employee>()
            // .HasIndex(o => o.PrimaryDepartmentId);

            modelBuilder.Entity<EmployeePosition>()
                .HasKey(o => new { o.EmployeeId, o.PositionId });

            modelBuilder.Entity<Group>()
               .HasIndex(o => o.Type);

            modelBuilder.Entity<GroupMember>()
               .HasKey(o => new { o.GroupId, o.EmployeeId });
        }

    }
}
