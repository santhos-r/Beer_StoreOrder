namespace Beer_StoreOrder.Model.Models;
public partial class BarBeerStockDetail
{
    public long? BarId { get; set; }

    public string? BarName { get; set; }

    public string? BeerName { get; set; }

    public long? AvailableStock { get; set; }

    public double? PercentageAlcoholByVolume { get; set; }
}
