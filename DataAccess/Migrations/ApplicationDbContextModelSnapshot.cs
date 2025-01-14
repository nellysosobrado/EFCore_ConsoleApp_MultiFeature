﻿// <auto-generated />
using System;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClassLibrary.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Calculator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CalculationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("FirstNumber")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Result")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double>("SecondNumber")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.HasKey("Id");

                    b.ToTable("Calculations");
                });

            modelBuilder.Entity("ClassLibrary.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ComputerMove")
                        .HasColumnType("int");

                    b.Property<DateTime>("GameDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("PlayerMove")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ClassLibrary.Models.Shape", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Area")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double?>("BaseLength")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)")
                        .HasColumnName("Base");

                    b.Property<DateTime>("CalculationDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Height")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double>("Perimeter")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<int>("ShapeType")
                        .HasColumnType("int");

                    b.Property<double?>("Side")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double?>("SideA")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double?>("SideB")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double?>("SideC")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.Property<double?>("Width")
                        .HasPrecision(18, 2)
                        .HasColumnType("float(18)");

                    b.HasKey("Id");

                    b.ToTable("Shapes");
                });
#pragma warning restore 612, 618
        }
    }
}
