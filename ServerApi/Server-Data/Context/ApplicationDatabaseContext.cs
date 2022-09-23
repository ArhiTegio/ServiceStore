using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server_Data.EntityDb;


namespace Server_Data.Context
{
    public class ApplicationDatabaseContext : Base.DbContextBase
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options) 
        {
        }

        public DbSet<Bayer> Bayer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProvidedProducts> ProvidedProducts { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SalesData> SalesData { get; set; }
        public DbSet<SalesPoint> SalesPoint { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bayer>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(p => p.SalesIds)
                    .WithOne(d => d.Bayer)
                    .HasForeignKey(d => d.BuyerId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(p => p.ProvidedProducts_)
                    .WithOne(d => d.Product_)
                    .HasForeignKey(d => d.ProductId);

                entity.HasMany(p => p.SalesData_)
                    .WithOne(d => d.Product_)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<ProvidedProducts>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.ProductQuantity)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasMany(p => p.SalesPointId)
                    .WithOne(d => d.ProvidedProducts_)
                    .HasForeignKey(d => d.ProvidedProductsId);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.Date)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.TotalAmount)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasMany(p => p.SalesPointId)
                    .WithOne(d => d.Sale)
                    .HasForeignKey(d => d.SaleId);

                entity.HasMany(p => p.SalesDataId)
                    .WithOne(d => d.Sale)
                    .HasForeignKey(d => d.SaleId);
            });

            modelBuilder.Entity<SalesData>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.ProductIdAmount)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ProductQuantity)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesPoint>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });
        }
    }
}

