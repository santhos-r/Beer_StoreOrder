using System;
using System.Collections.Generic;

namespace Northwind.Models;

public class BarBeerStock
{
    public long Id { get; set; }

    public long? ProductId { get; set; }

    public long? AvailableStock { get; set; }

    public long? BarId { get; set; }

    public Bar? Bar { get; set; }

    public Beer? Product { get; set; }
}
