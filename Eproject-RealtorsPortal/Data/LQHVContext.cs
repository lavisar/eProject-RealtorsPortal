using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Eproject_RealtorsPortal.Models;

namespace Eproject_RealtorsPortal.Data
{
    public partial class LQHVContext : DbContext
    {
        public LQHVContext()
        {
        }

        public LQHVContext(DbContextOptions<LQHVContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<BusinessType> BusinessTypes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackageType> PackageTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=LQHV;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.AdminImage).HasDefaultValueSql("('defaultImage.jpg')");

                entity.Property(e => e.AdminRole).HasDefaultValueSql("('Staff')");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasOne(d => d.Cities)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CitiesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_City_city_id");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.BusinessTypes)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.BusinessTypesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Category_businessTypes_id");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasOne(d => d.Regions)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.RegionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_City_region_id");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("fk_Contact_users_id");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ImageId).ValueGeneratedNever();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Image_Product");
                entity.Property(i => i.ImageId)
                     .UseIdentityColumn();
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsDate).HasDefaultValueSql("(getdate())");
                entity.HasOne(n => n.Image)
                   .WithOne(i => i.News)
                   .HasForeignKey<Image>(i => i.NewsId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.PackagesStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.PackageType)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.PackageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Packages_PackageTypes1");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentDatetime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_Payment_product_id");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Payment_users_id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("('verifying')");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Product_category_id");

                entity.HasOne(d => d.Packages)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.PackagesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Product_package_id");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("fk_Product_users_id");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasOne(d => d.Countries)
                    .WithMany(p => p.Regions)
                    .HasForeignKey(d => d.CountriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Region_country_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.PackagesId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UsersImage).HasDefaultValueSql("('defaultImage.jpg')");

                entity.HasOne(d => d.Packages)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PackagesId)
                    .HasConstraintName("fk_User_Package_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
