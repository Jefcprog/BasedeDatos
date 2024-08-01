using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BasedeDatos.OracleModels;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cantone> Cantones { get; set; }

    public virtual DbSet<Parroquia> Parroquias { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Regione> Regiones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("User Id=system;Password=123456789;Data Source=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Cantone>(entity =>
        {
            entity.HasKey(e => e.IdCan).HasName("SYS_C008331");

            entity.ToTable("CANTONES");

            entity.Property(e => e.IdCan)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_CAN");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("CAN_DESCRIP");
            entity.Property(e => e.IdProv)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_PROV");
            entity.Property(e => e.Region)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_REG");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("SYS_C008332");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("SYS_C008333");
        });

        modelBuilder.Entity<Parroquia>(entity =>
        {
            entity.HasKey(e => e.IdPar).HasName("SYS_C008336");

            entity.ToTable("PARROQUIAS");

            entity.Property(e => e.IdPar)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_PAR");
            entity.Property(e => e.IdCan)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_CAN");
            entity.Property(e => e.IdProv)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_PROV");
            entity.Property(e => e.Region)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_REG");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("PAR_DESCRIP");

            entity.HasOne(d => d.IdCanNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdCan)
                .HasConstraintName("SYS_C008337");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("SYS_C008338");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("SYS_C008339");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProv).HasName("SYS_C008317");

            entity.ToTable("PROVINCIAS");

            entity.Property(e => e.IdProv)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID_PROV");
            entity.Property(e => e.Region)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_REG");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("PROV_DESCRIP");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("SYS_C008318");
        });

        modelBuilder.Entity<Regione>(entity =>
        {
            entity.HasKey(e => e.Region).HasName("SYS_C008315");

            entity.ToTable("REGIONES");

            entity.Property(e => e.Region)
                .HasColumnType("NUMBER")
                .HasColumnName("ID_REG");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("REG_DESCRIP");
        });

        modelBuilder.HasSequence("PROVINCIAS_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
