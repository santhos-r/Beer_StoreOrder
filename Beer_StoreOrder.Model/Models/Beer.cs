namespace Beer_StoreOrder.Model.Models;
public class Beer
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public double? PercentageAlcoholByVolume { get; set; }
    public long? BreweryId { get; set; }
    public ICollection<BarBeer> BarBeers { get; set; } = new List<BarBeer>();
    public Brewery? Brewery { get; set; }
}
