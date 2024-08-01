using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BasedeDatos.MysqlModels;

public partial class BdmysqlContext : DbContext
{
    public BdmysqlContext()
    {
    }

    public BdmysqlContext(DbContextOptions<BdmysqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cantone> Cantones { get; set; }

    public virtual DbSet<Parroquia> Parroquias { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Regione> Regiones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;database=bdmysql;user=root;password=123456789;sslmode=None", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cantone>(entity =>
        {
            entity.HasKey(e => e.IdCan).HasName("PRIMARY");

            entity.ToTable("cantones");

            entity.HasIndex(e => e.IdProv, "id_prov");

            entity.HasIndex(e => e.Region, "id_reg");

            entity.Property(e => e.IdCan)
                .ValueGeneratedNever()
                .HasColumnName("id_can");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .HasColumnName("can_descrip");
            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("cantones_ibfk_1");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("cantones_ibfk_2");
        });

        modelBuilder.Entity<Parroquia>(entity =>
        {
            entity.HasKey(e => e.IdPar).HasName("PRIMARY");

            entity.ToTable("parroquias");

            entity.HasIndex(e => e.IdCan, "id_can");

            entity.HasIndex(e => e.IdProv, "id_prov");

            entity.HasIndex(e => e.Region, "id_reg");

            entity.Property(e => e.IdPar)
                .ValueGeneratedNever()
                .HasColumnName("id_par");
            entity.Property(e => e.IdCan).HasColumnName("id_can");
            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .HasColumnName("par_descrip");

            entity.HasOne(d => d.IdCanNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdCan)
                .HasConstraintName("parroquias_ibfk_1");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("parroquias_ibfk_2");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("parroquias_ibfk_3");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProv).HasName("PRIMARY");

            entity.ToTable("provincias");

            entity.HasIndex(e => e.Region, "id_reg");

            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .HasColumnName("prov_descrip");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("provincias_ibfk_1");
        });

        modelBuilder.Entity<Regione>(entity =>
        {
            entity.HasKey(e => e.Region).HasName("PRIMARY");

            entity.ToTable("regiones");

            entity.Property(e => e.Region)
                .ValueGeneratedNever()
                .HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("reg_descrip");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
