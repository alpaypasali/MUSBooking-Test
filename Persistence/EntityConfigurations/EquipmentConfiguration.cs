using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.ToTable("Equipments").HasKey(b => b.Id);

         
            builder.Property(b => b.Name).IsRequired();
            builder.Property(b => b.Amount).IsRequired();
            builder.HasIndex(b => b.Name).IsUnique();
            builder.Property(b => b.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");



            builder.HasMany(b => b.OrderEquipments);



           
        }
    }
   

   



}
