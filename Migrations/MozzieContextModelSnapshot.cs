﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MozzieAiSystems.Models;

namespace MozzieAiSystems.Migrations
{
    [DbContext(typeof(MozzieContext))]
    partial class MozzieContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MozzieAiSystems.Models.AlgattasCmsSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Expires");

                    b.Property<string>("SessionId");

                    b.Property<string>("Udid");

                    b.Property<DateTime>("UpdateDateTime");

                    b.HasKey("Id");

                    b.ToTable("AlgattasCmsSession");
                });

            modelBuilder.Entity("MozzieAiSystems.Models.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(1000);

                    b.Property<string>("Address")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<DateTime>("ReportDateTime");

                    b.Property<int?>("ReportUserId");

                    b.Property<string>("Type")
                        .HasMaxLength(50);

                    b.Property<string>("Uuid")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("MozzieAiSystems.Models.LocationFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasMaxLength(100);

                    b.Property<string>("FilePath");

                    b.Property<float>("FileSize");

                    b.Property<long>("LocationId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("LocationFile");
                });

            modelBuilder.Entity("MozzieAiSystems.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MozzieAiSystems.Models.LocationFile", b =>
                {
                    b.HasOne("MozzieAiSystems.Models.Location", "Location")
                        .WithMany("Files")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
