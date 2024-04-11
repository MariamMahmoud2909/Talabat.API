using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (_dbContext?.ProductBrands.Count() == 0)
			{
				var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData); // Converts from json to needed format [Vice Versa Serialize]

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
			if (_dbContext?.ProductCategories.Count() == 0)
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
			if (_dbContext?.Products.Count() == 0)
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
		}
	}
}