using System;
using System.Collections.Generic;

namespace Beer_StoreOrder.Model.Models;

public class BarBeer
{
    public long Id { get; set; }

    public long? BeerId { get; set; }

    public long? AvailableStock { get; set; }

    public long? BarId { get; set; }

    public Bar? Bar { get; set; }

    public Beer? Beer { get; set; }
}
