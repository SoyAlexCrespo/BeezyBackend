using System;
using BeezyBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BeezyBackend.DataAccess.DBContext
{
    public class BeezyCinemaContext : DbContext
    {
        public BeezyCinemaContext()
        {
        }

        public BeezyCinemaContext(DbContextOptions<BeezyCinemaContext> options)
            : base(options)
        {
        }

        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Session> Session { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OpenSince).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Cinema)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cinema_City");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.OriginalLanguage).HasMaxLength(255);

                entity.Property(e => e.OriginalTitle)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.GenreId });
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.CinemaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Cinema");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_Movie");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_Room");
            });
        }
    }
}
