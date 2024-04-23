using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ISSTTP.Data;

public partial class DbcarShopContext : DbContext
{
    public DbcarShopContext()
    {
    }

    public DbcarShopContext(DbContextOptions<DbcarShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administarator> Administarators { get; set; }

    public virtual DbSet<AdministratorDetail> AdministratorDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Сustomer> Сustomers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LittleWitch\\SQLEXPRESS; Database=DBCarShop; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administarator>(entity =>
        {
            entity.ToTable("Administarator");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<AdministratorDetail>(entity =>
        {
            entity.HasOne(d => d.Administrator).WithMany(p => p.AdministratorDetails)
                .HasForeignKey(d => d.AdministratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdministratorDetails_Administrator");

            entity.HasOne(d => d.Detail).WithMany(p => p.AdministratorDetails)
                .HasForeignKey(d => d.DetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdministratorDetails_Detail");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Info).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<CustomerDetail>(entity =>
        {
            entity.Property(e => e.Address)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerDetails)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerDetails_Customer");

            entity.HasOne(d => d.Detail).WithMany(p => p.CustomerDetails)
                .HasForeignKey(d => d.DetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerDetails_Detail");

            entity.HasOne(d => d.Status).WithMany(p => p.CustomerDetails)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerDetails_Status");
        });

        modelBuilder.Entity<Detail>(entity =>
        {
            entity.Property(e => e.Info).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.Details)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Details_Category");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Сustomer>(entity =>
        {
            entity.ToTable("Сustomer");

            entity.Property(e => e.Address).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
