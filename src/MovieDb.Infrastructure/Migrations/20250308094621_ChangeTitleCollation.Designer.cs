﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieDb.Infrastructure.DbContexts;

#nullable disable

namespace MovieDb.Infrastructure.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    [Migration("20250308094621_ChangeTitleCollation")]
    partial class ChangeTitleCollation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("MovieDb.Domain.DataModels.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalLanguage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Popularity")
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<decimal>("VoteAverage")
                        .HasColumnType("TEXT");

                    b.Property<int>("VoteCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Title");

                    b.ToTable("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
