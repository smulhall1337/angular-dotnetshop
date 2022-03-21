﻿using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(item => item.ItemOrdered, 
            itemOrdered => { itemOrdered.WithOwner(); });

        builder.Property(item => item.Price).HasColumnType("decimal(18,2)");
    }
}