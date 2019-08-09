﻿// <auto-generated />
using System;
using EnterpriseContact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnterpriseContact.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    [Migration("20190612063259_adddatasourcetypefield")]
    partial class adddatasourcetypefield
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EnterpriseContact.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataSourceType");

                    b.Property<int?>("IconId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("Sort");

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("Number");

                    b.HasIndex("ParentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<int>("DataSourceType");

                    b.Property<int>("Gender");

                    b.Property<string>("IdCardNo")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Mobile")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("PrimaryDepartmentId");

                    b.Property<Guid>("PrimaryPositionId");

                    b.Property<Guid?>("UserId");

                    b.Property<string>("UserName")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("Number");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.EmployeePosition", b =>
                {
                    b.Property<Guid>("EmployeeId");

                    b.Property<Guid>("PositionId");

                    b.Property<int>("DataSourceType");

                    b.Property<bool>("IsPrimary");

                    b.HasKey("EmployeeId", "PositionId");

                    b.HasIndex("PositionId");

                    b.ToTable("EmployeePositions");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<Guid>("CreatedBy");

                    b.Property<int?>("IconId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Remark");

                    b.Property<int>("Type");

                    b.Property<DateTimeOffset>("Updated");

                    b.HasKey("Id");

                    b.HasIndex("Type");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.GroupMember", b =>
                {
                    b.Property<Guid>("GroupId");

                    b.Property<Guid>("EmployeeId");

                    b.Property<bool>("IsOwner");

                    b.Property<DateTimeOffset>("Joined");

                    b.HasKey("GroupId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataSourceType");

                    b.Property<Guid>("DepartmentId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("EnterpriseContactService.Entities.MdmDataHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<string>("HistoryVersion")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<DateTimeOffset>("SyncTime");

                    b.HasKey("Id");

                    b.ToTable("MdmDataHistories");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.Department", b =>
                {
                    b.HasOne("EnterpriseContact.Entities.Department", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("EnterpriseContact.Entities.EmployeePosition", b =>
                {
                    b.HasOne("EnterpriseContact.Entities.Employee", "Employee")
                        .WithMany("Positions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EnterpriseContact.Entities.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseContact.Entities.GroupMember", b =>
                {
                    b.HasOne("EnterpriseContact.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EnterpriseContact.Entities.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseContact.Entities.Position", b =>
                {
                    b.HasOne("EnterpriseContact.Entities.Department", "Department")
                        .WithMany("Positions")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
