using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BasedeDatos.SqlServerModels;

public partial class BdsqlserverContext : DbContext
{
    public BdsqlserverContext()
    {
    }

    public BdsqlserverContext(DbContextOptions<BdsqlserverContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cantone> Cantones { get; set; }

    public virtual DbSet<Parroquia> Parroquias { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Regione> Regiones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UAC8OOF\\SQLEXPRESS;Database=bdsqlserver;User ID=sa;Password=jefc2000;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cantone>(entity =>
        {
            entity.HasKey(e => e.IdCan).HasName("PK__Cantones__D54686D87742D8C5");

            entity.Property(e => e.IdCan)
                .ValueGeneratedNever()
                .HasColumnName("id_can");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("can_descrip");
            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("FK__Cantones__id_pro__440B1D61");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Cantones)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("FK__Cantones__id_reg__44FF419A");
        });

        modelBuilder.Entity<Parroquia>(entity =>
        {
            entity.HasKey(e => e.IdPar).HasName("PK__Parroqui__6FCA2A6ED2F7AE36");

            entity.Property(e => e.IdPar)
                .ValueGeneratedNever()
                .HasColumnName("id_par");
            entity.Property(e => e.IdCan).HasColumnName("id_can");
            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("par_descrip");

            entity.HasOne(d => d.IdCanNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdCan)
                .HasConstraintName("FK__Parroquia__id_ca__47DBAE45");

            entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.IdProv)
                .HasConstraintName("FK__Parroquia__id_pr__48CFD27E");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Parroquia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("FK__Parroquia__id_re__49C3F6B7");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProv).HasName("PK__Provinci__0DA3485DE80A240D");

            entity.Property(e => e.IdProv).HasColumnName("id_prov");
            entity.Property(e => e.Region).HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("prov_descrip");

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.Region)
                .HasConstraintName("FK__Provincia__id_re__412EB0B6");
        });

        modelBuilder.Entity<Regione>(entity =>
        {
            entity.HasKey(e => e.Region).HasName("PK__Regiones__6ABE6F0C13A9D314");

            entity.Property(e => e.Region)
                .ValueGeneratedNever()
                .HasColumnName("id_reg");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("reg_descrip");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
