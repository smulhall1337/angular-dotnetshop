﻿using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        // A protected member of a base class is accessible in a derived class only if the access occurs through the derived class type.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // when we create a migration, OnModelCreation() is whats called to build it
            // we override it here and call it passing out configurations 
            base.OnModelCreating(modelBuilder);
            // we're calling the base class, the DbContext Class, and passing in the model builder
            // then we can specify configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                /*
                 * Sqlite doesn't support ordering by decimals
                 * so we need to convert the price to a double
                 * this wont be necessary with SQL
                 */
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    IEnumerable<PropertyInfo> properties = entityType.ClrType
                            .GetProperties().Where(p => p.PropertyType == typeof(decimal?));
                    foreach (PropertyInfo property in properties)
                    {
                        modelBuilder.Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion<double>();
                    }
                }

            }
        }
    }
}
