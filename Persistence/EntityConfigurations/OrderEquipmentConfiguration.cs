using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class OrderEquipmentConfiguration : IEntityTypeConfiguration<OrderEquipment>
    {
        public void Configure(EntityTypeBuilder<OrderEquipment> builder)
        {
            builder.ToTable("OrderEquipments").HasKey(b => new { b.OrderId, b.EquipmentId });
            builder.HasOne(oe => oe.Order)
          .WithMany(o => o.OrderEquipments)
          .HasForeignKey(oe => oe.OrderId);


            builder.HasOne(oe => oe.Equipment)
                .WithMany(e => e.OrderEquipments)
                .HasForeignKey(oe => oe.EquipmentId);

        }
    }
}
