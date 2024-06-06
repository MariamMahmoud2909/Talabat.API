using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config.Order_Config
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

			builder.Property(O => O.Status)
				   .HasConversion(
				   (OStatus) => OStatus.ToString(),
				   (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
				   );

			builder.HasOne(O => O.DeliveryMethod)
				   .WithMany()
				   .OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(O => O.Items)
				   .WithOne()
				   .OnDelete(DeleteBehavior.Cascade);

			builder.Property(O => O.SubTotal)
					.HasColumnType("decimal(12,2)");
		}
	}
}
