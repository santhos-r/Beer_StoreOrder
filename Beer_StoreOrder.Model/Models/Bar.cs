namespace Beer_StoreOrder.Model.Models;
public partial class Bar
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<BarBeerStock> BarBeerStocks { get; set; } = new List<BarBeerStock>();
}
