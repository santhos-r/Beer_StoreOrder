namespace Beer_StoreOrder.Model;
public partial class Beer
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public double? PercentageAlcoholByVolume { get; set; }

    public long? BreweryId { get; set; }

    public virtual ICollection<BarBeerStock> BarBeerStocks { get; set; } = new List<BarBeerStock>();

    public virtual Brewery? Brewery { get; set; }
}
