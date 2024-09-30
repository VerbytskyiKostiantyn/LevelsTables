﻿// <auto-generated />
using System;
using LevelsTables.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LevelsTables.Migrations
{
    [DbContext(typeof(LevelsDbContext))]
    [Migration("20240930103739_crateDbAddCalibrationsTanks")]
    partial class crateDbAddCalibrationsTanks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlisonicLevels.Models.Calibration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Level")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TankId")
                        .HasColumnType("int");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("modificator")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ratio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("TankId");

                    b.ToTable("Calibrations");
                });

            modelBuilder.Entity("Levels.Models.Tank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal?>("Alert_Level")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ExternalProbeId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("FuelID")
                        .HasColumnType("int");

                    b.Property<decimal?>("MaxVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProbeSerial")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Probetype")
                        .HasColumnType("int");

                    b.Property<decimal?>("Product_zero")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StationID")
                        .HasColumnType("int");

                    b.Property<int>("TankNumber")
                        .HasColumnType("int");

                    b.Property<string>("TankUID")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal?>("Water_zero")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Tanks");
                });

            modelBuilder.Entity("AlisonicLevels.Models.Calibration", b =>
                {
                    b.HasOne("Levels.Models.Tank", "Tank")
                        .WithMany("Calibrations")
                        .HasForeignKey("TankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tank");
                });

            modelBuilder.Entity("Levels.Models.Tank", b =>
                {
                    b.Navigation("Calibrations");
                });
#pragma warning restore 612, 618
        }
    }
}
