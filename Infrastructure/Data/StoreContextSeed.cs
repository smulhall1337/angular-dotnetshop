using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // check and see if theres any brands in the DB
                if (!context.ProductBrands.Any())
                {
                    // if not...grab it from seed data
                    string brandsData = File.ReadAllText("../Infrastructure/Data/Seed Data/brands.json");
                    List<ProductBrand> brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    // then add it to the DB via the context
                    foreach (ProductBrand item in brands)
                    {
                        context.ProductBrands.Add(item);
                        await context.SaveChangesAsync();
                    }
                }
                // do the same for types
                if (!context.ProductTypes.Any())
                {
                    string typesData = File.ReadAllText("../Infrastructure/Data/Seed Data/types.json");
                    List<ProductType> types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (ProductType item in types)
                    {
                        context.ProductTypes.Add(item);
                        await context.SaveChangesAsync();
                    }
                }
                if (!context.Products.Any())
                {
                    string productsData = File.ReadAllText("../Infrastructure/Data/Seed Data/products.json");
                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (Product item in products)
                    {
                        context.Products.Add(item);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                ILogger<StoreContextSeed> logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
