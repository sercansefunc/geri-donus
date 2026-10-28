using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeriDonus.Data.Entities;

public partial class GeriDonusContext : DbContext
{
    public GeriDonusContext(DbContextOptions<GeriDonusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aile> Ailes { get; set; }

    public virtual DbSet<Basvuru> Basvurus { get; set; }

    public virtual DbSet<BasvuruDurumGecmisi> BasvuruDurumGecmisis { get; set; }

    public virtual DbSet<Durum> Durums { get; set; }

    public virtual DbSet<Il> Ils { get; set; }

    public virtual DbSet<Kisi> Kisis { get; set; }

    public virtual DbSet<Personel> Personels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aile>(entity =>
        {
            entity.ToTable("Aile");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Basvuru>(entity =>
        {
            entity.ToTable("Basvuru");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Basvurutarihi)
                .HasDefaultValueSql("(sysdatetime())", "DF_Basvuru_basvurutarihi")
                .HasColumnName("basvurutarihi");
            entity.Property(e => e.KisiId).HasColumnName("kisi_id");
            entity.Property(e => e.MudurlukIlId).HasColumnName("mudurluk_il_id");

            entity.HasOne(d => d.Kisi).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.KisiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Basvuru_Kisi");

            entity.HasOne(d => d.MudurlukIl).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.MudurlukIlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Basvuru_Il");
        });

        modelBuilder.Entity<BasvuruDurumGecmisi>(entity =>
        {
            entity.ToTable("BasvuruDurumGecmisi");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.DurumId).HasColumnName("durum_id");
            entity.Property(e => e.Gerekce)
                .HasMaxLength(500)
                .HasColumnName("gerekce");
            entity.Property(e => e.Islemzamani)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())", "DF_BasvuruDurumGecmisi_islemzamani")
                .HasColumnName("islemzamani");
            entity.Property(e => e.PersonelId).HasColumnName("personel_id");

            entity.HasOne(d => d.Basvuru).WithMany(p => p.BasvuruDurumGecmisis)
                .HasForeignKey(d => d.BasvuruId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BasvuruDurumGecmisi_Basvuru");

            entity.HasOne(d => d.Durum).WithMany(p => p.BasvuruDurumGecmisis)
                .HasForeignKey(d => d.DurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BasvuruDurumGecmisi_Durum");

            entity.HasOne(d => d.Personel).WithMany(p => p.BasvuruDurumGecmisis)
                .HasForeignKey(d => d.PersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BasvuruDurumGecmisi_Personel");
        });

        modelBuilder.Entity<Durum>(entity =>
        {
            entity.ToTable("Durum");

            entity.HasIndex(e => e.Ad, "UQ_Durum_ad").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ad)
                .HasMaxLength(30)
                .HasColumnName("ad");
        });

        modelBuilder.Entity<Il>(entity =>
        {
            entity.ToTable("Il");

            entity.HasIndex(e => e.Ad, "UQ_Il_ad").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ad)
                .HasMaxLength(20)
                .HasColumnName("ad");
        });

        modelBuilder.Entity<Kisi>(entity =>
        {
            entity.ToTable("Kisi");

            entity.HasIndex(e => e.Kimlikno, "UX_Kisi_kimlikno")
                .IsUnique()
                .HasFilter("([kimlikno] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.Adres)
                .HasMaxLength(200)
                .HasColumnName("adres");
            entity.Property(e => e.AileId).HasColumnName("aile_id");
            entity.Property(e => e.Dogumtarihi).HasColumnName("dogumtarihi");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Kimlikno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("kimlikno");
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .HasColumnName("soyad");
            entity.Property(e => e.Telno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telno");

            entity.HasOne(d => d.Aile).WithMany(p => p.Kisis)
                .HasForeignKey(d => d.AileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kisi_Aile");

            entity.HasOne(d => d.Il).WithMany(p => p.Kisis)
                .HasForeignKey(d => d.IlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kisi_Il");
        });

        modelBuilder.Entity<Personel>(entity =>
        {
            entity.ToTable("Personel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasColumnName("rol");
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .HasColumnName("soyad");

            entity.HasOne(d => d.Il).WithMany(p => p.Personels)
                .HasForeignKey(d => d.IlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Personel_Il");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
