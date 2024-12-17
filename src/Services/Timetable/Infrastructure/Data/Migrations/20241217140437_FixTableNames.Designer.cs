﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241217140437_FixTableNames")]
    partial class FixTableNames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Appointment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("PatientId")
                        .HasColumnType("bigint")
                        .HasColumnName("patient_id");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time");

                    b.Property<long>("TimetableId")
                        .HasColumnType("bigint")
                        .HasColumnName("timetable_id");

                    b.HasKey("Id");

                    b.HasIndex("TimetableId");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("Domain.Models.Timetable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("DoctorId")
                        .HasColumnType("bigint")
                        .HasColumnName("doctor_id");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("from");

                    b.Property<long>("HospitalId")
                        .HasColumnType("bigint")
                        .HasColumnName("hospital_id");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("room");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("to");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("NOW()");

                    b.HasKey("Id");

                    b.ToTable("timetables");
                });

            modelBuilder.Entity("Domain.Models.Appointment", b =>
                {
                    b.HasOne("Domain.Models.Timetable", "Timetable")
                        .WithMany("Appointments")
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Timetable");
                });

            modelBuilder.Entity("Domain.Models.Timetable", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
