using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (!_dbContext.ProductBrands.Any())
			{
				var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

				if (Brands?.Count > 0)
				{
					foreach (var Brand in Brands)
					{
						Brand.Id = 0;
						await _dbContext.AddAsync(Brand);
					}
					await _dbContext.SaveChangesAsync();
				}

			}
			if (!_dbContext.ProductCategories.Any())
			{
				var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
				var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

				if (Categories?.Count > 0)
				{
					foreach (var Category in Categories)
					{
						Category.Id = 0;
						await _dbContext.AddAsync(Category);
					}
					await _dbContext.SaveChangesAsync();
				}

			}
			if (!_dbContext.Products.Any())
			{
				var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

				if (Products?.Count > 0)
				{
					foreach (var Product in Products)
					{
						Product.Id = 0;
						await _dbContext.AddAsync(Product);
					}
					var count = await _dbContext.SaveChangesAsync();
				}

			}
			if (!_dbContext.DeliveryMethods.Any())
			{
				var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

				if (deliveryMethods?.Count > 0)
				{
					foreach (var deliveryMethod in deliveryMethods)
					{
						deliveryMethod.Id = 0;
						await _dbContext.AddAsync(deliveryMethod);
					}
					var count = await _dbContext.SaveChangesAsync();
				}

			}
		}
	}
}
