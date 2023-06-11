using System;
using System.Collections.Generic;

namespace Beer_StoreOrder.Model.Models;

public class Bar
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public ICollection<BarBeer> BarBeers { get; set; } = new List<BarBeer>();
}
