﻿namespace Beer_StoreOrder.Model.Models;
public class Bar
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public ICollection<BarBeerStock> BarBeerStocks { get; set; } = new List<BarBeerStock>();
}
