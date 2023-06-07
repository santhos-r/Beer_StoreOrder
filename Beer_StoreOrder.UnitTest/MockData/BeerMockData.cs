using Beer_StoreOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Sdk;

namespace Beer_StoreOrder.UnitTest.MockData
{
    public static class BeerMockData
    {
        public static List<Beer> GetBeer()
        {
            List<Beer> BeerData = new List<Beer>
            {
                new Beer{
                    Id= 5,
                    Name= "Carlsberg Unfiltered",
                    PercentageAlcoholByVolume= 4.5,
                    BreweryId= 1
                    },
                    new Beer{
                    Id= 9,
                    Name= "Fosters Lager",
                    PercentageAlcoholByVolume= 4,
                    BreweryId= 3
                    },
                    new Beer{
                    Id= 10,
                    Name="Stella Artois Lager",
                    PercentageAlcoholByVolume= 5,
                    BreweryId= 7
                    },
                    new Beer{
                    Id= 11,
                    Name= "Stella Artois Petite Premium Lager",
                    PercentageAlcoholByVolume= 5,
                    BreweryId= 7
                    },
                    new Beer{
                    Id= 13,
                    Name= "Guinness Cold Brew Coffee Beer",
                    PercentageAlcoholByVolume= 4,
                    BreweryId= 4
                    },
                    new Beer{
                    Id= 16,
                    Name= "Guinness Original",
                    PercentageAlcoholByVolume= 4.2,
                    BreweryId= 4
                    },
                    new Beer{
                    Id= 17,
                    Name= "River Amstel",
                    PercentageAlcoholByVolume= 4.1,
                    BreweryId= 5

                }
            };
            return BeerData;
        }


        public static List<Brewery> GetBreweries()
        {
            List<Brewery> BreweryData = new List<Brewery>
            {
                new Brewery{
                    Id= 1,
                    Name= "Carlsberg Unfiltered",
                },

                new Brewery {
                     Id= 2,
                    Name="Cobra Group",

                 },

                new Brewery{
                     Id= 3,
                    Name= "Fosters Groups",

                }

            };
            return BreweryData;
        }
    }
}

