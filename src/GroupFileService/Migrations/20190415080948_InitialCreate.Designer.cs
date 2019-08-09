﻿// <auto-generated />
using System;
using GroupFile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GroupFile.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    [Migration("20190415080948_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GroupFile.Entities.FileItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DownloadAmount");

                    b.Property<Guid>("GroupId");

                    b.Property<string>("Name");

                    b.Property<string>("StoreId");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.Property<Guid>("UploaderId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("FileItems");
                });
#pragma warning restore 612, 618
        }
    }
}