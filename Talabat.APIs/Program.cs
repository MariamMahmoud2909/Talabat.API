using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
	public class Program
	{
		// Entry Point
		public static void Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the Dependency Injection container.

			webApplicationBuilder.Services.AddControllers();
			// Register the required web API services to the DI Container

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();
			// Register services required to document APIs [automatically using swagger]

			webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")); //Connection String
			});

			#endregion

			var app = webApplicationBuilder.Build(); // Web Application

			#region Configure Kestrel Middlewares
			// Configure the HTTP request pipeline. [Middleware]
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}