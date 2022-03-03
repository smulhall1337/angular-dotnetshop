using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    /**
     * Config classes like this let us add different properties to
     * entities. Is something required, does something have to be null, stuff like that.
     * Classes like this make use of IEntityTypeConfiguration.
     */
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();// required
            builder.Property(p => p.Name).IsRequired()
                .HasMaxLength(100);// required, max length of 100 characters
            builder.Property(p => p.Description).IsRequired()
                .HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)"); // our price will have 2 decimal places
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);// entity framework does this automatically (because we specified it in product) 
                                                      // but we're just doing it here to demonstrate a different way
        }
    }
}
