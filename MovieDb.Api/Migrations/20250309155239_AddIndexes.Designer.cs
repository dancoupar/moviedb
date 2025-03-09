﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieDb.Api.DbContexts;

#nullable disable

namespace MovieDb.Api.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    [Migration("20250309155239_AddIndexes")]
    partial class AddIndexes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("MovieDb.Api.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("MovieDb.Api.Entities.MovieActor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActorName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ActorName");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieActor");
                });

            modelBuilder.Entity("MovieDb.Api.Entities.MovieGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Genre");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieGenre");
                });

            modelBuilder.Entity("MovieDb.Api.Entities.MovieActor", b =>
                {
                    b.HasOne("MovieDb.Api.Entities.Movie", "Movie")
                        .WithMany("Actors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieDb.Api.Entities.MovieGenre", b =>
                {
                    b.HasOne("MovieDb.Api.Entities.Movie", "Movie")
                        .WithMany("Genres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieDb.Api.Entities.Movie", b =>
                {
                    b.Navigation("Actors");

                    b.Navigation("Genres");
                });
#pragma warning restore 612, 618
        }
    }
}
