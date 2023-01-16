using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class OrderedProductConfiguration : IEntityTypeConfiguration<OrderedProduct>
    {
        public void Configure(EntityTypeBuilder<OrderedProduct> builder)
        {
            builder
                .ToTable("OrderedProduct");

            builder
              .HasOne(op => op.Order)
              .WithMany(o => o.OrderedProducts)
              .HasForeignKey(op => op.OrderId);

            builder
              .HasOne(op => op.Book)
              .WithMany(b => b.OrderedProducts)
              .HasForeignKey(op => op.BookId);
        }
    }
}