﻿// <auto-generated />
using System;
using DemoLambdaFunction.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DemoLambdaFunction.Migrations
{
    [DbContext(typeof(SurveyDbContext))]
    [Migration("20250123092828_InitialSurveySchema")]
    partial class InitialSurveySchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DemoLambdaFunction.Models.SurveyResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyRecommendation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GrowthOpportunities")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ManagerSupport")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoleSatisfaction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ValueRecognition")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SurveyResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
