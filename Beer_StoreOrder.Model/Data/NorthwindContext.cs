using Beer_StoreOrder.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Beer_StoreOrder.Model.Data;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Bar> Bars { get; set; }

    public virtual DbSet<BarBeerStock> BarBeerStocks { get; set; }

    public virtual DbSet<BarBeerStockDetail> BarBeerStockDetails { get; set; }

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

        modelBuilder.Entity<BarBeerStock>(entity =>
        {
            entity.ToTable("BarBeerStock");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AvailableStock).HasColumnType("INTERGER");

            entity.HasOne(d => d.Bar).WithMany(p => p.BarBeerStocks).HasForeignKey(d => d.BarId);

            entity.HasOne(d => d.Product).WithMany(p => p.BarBeerStocks).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<BarBeerStockDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BarBeerStockDetails");

            entity.Property(e => e.AvailableStock).HasColumnType("INTERGER");
            entity.Property(e => e.BarId).HasColumnName("BarID");
            entity.Property(e => e.BarName).HasColumnType("VARCHAR");
            entity.Property(e => e.BeerName).HasColumnType("VARCHAR(8000)");
            entity.Property(e => e.PercentageAlcoholByVolume).HasColumnType("DOUBLE");
        });

        modelBuilder.Entity<Beer>(entity =>
        {
            entity.ToTable("Beer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("VARCHAR(8000)");
            entity.Property(e => e.PercentageAlcoholByVolume).HasColumnType("DOUBLE");

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
