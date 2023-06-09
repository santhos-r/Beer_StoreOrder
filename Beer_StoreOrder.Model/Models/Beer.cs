namespace Beer_StoreOrder.Model.Models;
public class Beer
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public double? PercentageAlcoholByVolume { get; set; }

    public long? BreweryId { get; set; }

    public ICollection<BarBeerStock> BarBeerStocks { get; set; } = new List<BarBeerStock>();
    
    public Brewery? Brewery { get; set; }
}
