using System;
using System.Collections.Generic;
using FourSquares.Models;
using Microsoft.EntityFrameworkCore;

namespace FourSquares.Data;

public partial class FoursquareContext : DbContext
{
    public FoursquareContext()
    {
    }
    public FoursquareContext(DbContextOptions<FoursquareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hotel> Hotels { get; set; }
   
    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DIVYARBHAT-W11;Database=FourSquare;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.HotelId).HasName("PK__Hotels__46023BDF4E6C9EB0");

            entity.HasIndex(e => e.City, "IX_Hotels_City");

            entity.HasIndex(e => e.HotelName, "IX_Hotels_HotelName");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.HotelName).HasMaxLength(200);
            entity.Property(e => e.Timings).HasMaxLength(100);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79CE1C3ED3AE");

            entity.HasIndex(e => e.HotelId, "IX_Reviews_HotelId");

            entity.HasIndex(e => e.UserId, "IX_Reviews_UserId");

            entity.HasIndex(e => new { e.UserId, e.HotelId }, "IX_Reviews_UserId_HotelId");

            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__HotelId__3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__DateAdd__3D5E1FD2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C1D7A0590");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.UserName, "IX_Users_UserName");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345C877A20").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.Role).IsRequired().HasDefaultValue("User");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
