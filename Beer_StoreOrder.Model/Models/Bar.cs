using System;
using System.Collections.Generic;

namespace Northwind.Models;
public class Bar
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public ICollection<BarBeerStock> BarBeerStocks { get; set; } = new List<BarBeerStock>();
}
