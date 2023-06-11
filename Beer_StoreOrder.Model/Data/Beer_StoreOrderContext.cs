using System;
using System.Collections.Generic;
using Beer_StoreOrder.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.Model.Data;

public partial class Beer_StoreOrderContext : DbContext
{
    public Beer_StoreOrderContext(DbContextOptions<Beer_StoreOrderContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bar> Bars { get; set; }

    public virtual DbSet<BarBeer> BarBeers { get; set; }

    public virtual DbSet<Beer> Beers { get; set; }

    public virtual DbSet<Brewery> Breweries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bar>(entity =>
        {
            entity.ToTable("Bar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasColumnType("VARCHAR");
            entity.Property(e => e.Name).HasColumnType("VARCHAR");
        });

        modelBuilder.Entity<BarBeer>(entity =>
        {
            entity.ToTable("BarBeer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AvailableStock).HasColumnType("INTERGER");

            entity.HasOne(d => d.Bar).WithMany(p => p.BarBeers).HasForeignKey(d => d.BarId);

            entity.HasOne(d => d.Beer).WithMany(p => p.BarBeers).HasForeignKey(d => d.BeerId);
        });

        modelBuilder.Entity<Beer>(entity =>
        {
            entity.ToTable("Beer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("VARCHAR(8000)");
            entity.Property(e => e.PercentageAlcoholByVolume)
                .HasDefaultValueSql("0")
                .HasColumnType("DOUBLE");

            entity.HasOne(d => d.Brewery).WithMany(p => p.Beers).HasForeignKey(d => d.BreweryId);
        });

        modelBuilder.Entity<Brewery>(entity =>
        {
            entity.ToTable("Brewery");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("VARCHAR(8000)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
