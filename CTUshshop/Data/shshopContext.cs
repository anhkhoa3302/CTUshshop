using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CTUshshop.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CTUshshop.Data
{
    public partial class shshopContext : DbContext
    {
        public shshopContext()
        {
        }

        public shshopContext(DbContextOptions<shshopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Evidence> Evidence { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductImg> ProductImg { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportCategory> ReportCategory { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Ward> Ward { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CTUshshop;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.ProfilePic).IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AspNetUsers_district");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AspNetUsers_province");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.WardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AspNetUsers_ward");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasOne(d => d.Province)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_district_province");
            });

            modelBuilder.Entity<Evidence>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Evidence)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_evidence_report");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_message_AspNetUsers");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_AspNetUsers");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_item_order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_item_product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_AspNetUsers");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ProductCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_category_category");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCategory)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_category_product");
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Img).IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImg)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_product_img_product");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.HasOne(d => d.ReportCategory)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.ReportCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_report_report_category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_report_AspNetUsers");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_order");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transaction_AspNetUsers");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasOne(d => d.District)
                    .WithMany(p => p.Ward)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ward_district");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
