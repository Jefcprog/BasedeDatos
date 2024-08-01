using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BasedeDatos.PostgresqlModels
{
    public partial class BdpostgresqlContext : DbContext
    {
        public BdpostgresqlContext()
        {
        }

        public BdpostgresqlContext(DbContextOptions<BdpostgresqlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cantone> Cantones { get; set; }
        public virtual DbSet<Parroquia> Parroquias { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<Regione> Regiones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=bdpostgresql;Username=postgres;Password=123456789");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cantone>(entity =>
            {
                entity.HasKey(e => e.IdCan).HasName("cantones_pkey");

                entity.ToTable("cantones");

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
                    .HasConstraintName("cantones_id_prov_fkey");

                entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Cantones)
                    .HasForeignKey(d => d.Region)
                    .HasConstraintName("cantones_id_reg_fkey");
            });

            modelBuilder.Entity<Parroquia>(entity =>
            {
                entity.HasKey(e => e.IdPar).HasName("parroquias_pkey");

                entity.ToTable("parroquias");

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
                    .HasConstraintName("parroquias_id_can_fkey");

                entity.HasOne(d => d.IdProvNavigation).WithMany(p => p.Parroquia)
                    .HasForeignKey(d => d.IdProv)
                    .HasConstraintName("parroquias_id_prov_fkey");

                entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Parroquia)
                    .HasForeignKey(d => d.Region)
                    .HasConstraintName("parroquias_id_reg_fkey");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.HasKey(e => e.IdProv).HasName("provincias_pkey");

                entity.ToTable("provincias");

                entity.Property(e => e.IdProv).HasColumnName("id_prov");
                entity.Property(e => e.Region).HasColumnName("id_reg");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(70)
                    .HasColumnName("prov_descrip");

                entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Provincia)
                    .HasForeignKey(d => d.Region)
                    .HasConstraintName("provincias_id_reg_fkey");
            });

            modelBuilder.Entity<Regione>(entity =>
            {
                entity.HasKey(e => e.Region).HasName("regiones_pkey");

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
}
