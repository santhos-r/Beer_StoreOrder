using System;
using System.Collections.Generic;

namespace Beer_StoreOrder.Model.Models;

public class Brewery
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Beer> Beers { get; set; } = new List<Beer>();
}
