# Beer StoreOrder
This project is created to produce a .NET Core Web Api which stores, updates and retrieves different types of beer, their associated breweries and bars 
that serve the beers & included with unit tests.

## DB Structure
### Beer
| Property                  | Type    |
| ------------------------- | ------- |
| Name                      | string  |
| PercentageAlcoholByVolume | decimal |

### Brewery
| Property                  | Type    |
| ------------------------- | ------- |
| Name                      | string  |

### Bar
| Property                  | Type    |
| ------------------------- | ------- |
| Name                      | string  |
| Address                   | string  |

## Technical Stacks 
Below are the Technical Stacks used in this project.
| Technical Stacks                  |
| ------------------------- |
| Web API Core 6.0 | 
| EF 7.0.5 DB First Approach | 
| Scaffold SQLite Database | 
| Dependency Injection | 
| Linq | 
| Swagger - Validate API | 
| Unit Testing | 

## To Run this application
Below are the instruction to be followed to run this application.
* Download the complete source from the GitHub Page.
* Download the SQLite database from 
https://github.com/santhos-r/Beer_StoreOrder/tree/master/Beer_StoreOrder.Modal/Database => Beer_StoreOrder.sqlite
or go to Beer_StoreOrder.Modal Source and then go to Database/Beer_StoreOrder.sqlite
* create a new folder in local system and configure the DB Path in the appsettings in VS solution (instead of "E:/SQLLite_DBFile/Beer_StoreOrder.sqlite" you can configure with your       local SQLite DB path in the "Beer_StoreOrder.Api" & "Beer_StoreOrder.Model" project.
   ##### "ConnectionStrings": {"Beer_StoreOrderDB": "Data source=E:/DB File/E:/SQLLite_DBFile/Beer_StoreOrder.sqlite" }
* DBeaver software to be installed in the local system and configure the SQLite database which you placed in your local system path.
* Once the above configuration is done now you are good to run with the solution.

## Additional Commands 
If you are facing any issues while running the source codes, execute the below commands in the Nuget Package Manager =>Package Manager Console

* Packages to install =>
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

* Install & Update dotnet EF tool =>
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

* Scaffold SQLite Database =>
dotnet ef dbcontext scaffold Name=Beer_StoreOrderDB Microsoft.EntityFrameworkCore.Sqlite --output-dir Models --context-dir Data --namespace Beer_StoreOrder.Model.Models --context-namespace Beer_StoreOrder.Model.Data --context Beer_StoreOrderContext -f --no-onconfiguring

## Swagger samples 

The samples for posting and updating the related endpoints are provided below.
### Beer - Post & Put Method
```bash
{									
  "id": 38,						
  "name": "Stella Artois Lager",
  "percentageAlcoholByVolume": 5,
  "breweryId": 7					
}	
```
### Brewery - Post & Put Method
```bash
{
  "id": 101,
  "name": "Fosters Group SR"
}
```
### Brewery Beers - Post Method
```bash
{
  "id": 10,
  "name": "Stella Artois Lager",
  "percentageAlcoholByVolume": 5,
  "breweryId": 7
}
```

### Bar - Post & Put Method
```bash
{
    "id": 10,
    "name": "Happiness Forgets",
    "address": "8-9 Hoxton Square, London N1 6NU"
}
```
### BarBar - Post Method
```bash
{
  "id": 101,
  "name": "Carlsberg Unfiltered 4.5%",
  "percentageAlcoholByVolume": 4.5,
  "breweryId": 10
}
```