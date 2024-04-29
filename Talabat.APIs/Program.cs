using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.APIs.Helpers;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Middlewares;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Extensions;
using StackExchange.Redis;

namespace Talabat.APIs
{
	public class Program
	{
		// Entry Point
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the Dependency Injection container.

			webApplicationBuilder.Services.AddControllers();
			// Register the required web API services to the DI Container

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerService();
            // Register services required to document APIs [automatically using swagger]

            webApplicationBuilder.Services.AddApplicationsService();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"))/*.UseLazyLoadingProxies()*/; //Connection String
			});

            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			}
			);

			webApplicationBuilder.Services.AddApplicationsService();
			#endregion

			var app = webApplicationBuilder.Build(); // Web Application

			#region Update-Database

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>(); // Ask CLR for creating object from DbContext Class Explicitly

            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			var logger = loggerFactory.CreateLogger<Program>();

			try
			{
				await _dbContext.Database.MigrateAsync(); //Update-Database [To Build the database on deploying and running the project]
				await StoreContextSeed.SeedAsync(_dbContext);

                await _identityDbContext.Database.MigrateAsync();
            }
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error has occured during apply the migration");
			}
			#endregion

			#region Configure Kestrel Middlewares
			// Configure the HTTP request pipeline. [Middleware]

			//app.UseMiddleware<ExceptionMiddleware>();

			app.Use(async (httpContext, _next) =>
			{
				try
				{
					// Take an action with the request
					await _next.Invoke(httpContext); // Go to next middleware
													 // Take an action with the response
				}
				catch (Exception ex)
				{
					logger.LogError(ex.Message); // development environment
					httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					httpContext.Response.ContentType = "application/json";
					
					var response = app.Environment.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
					
					var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
					var json = JsonSerializer.Serialize(response, options);
					
					await httpContext.Response.WriteAsync(json);
				}
			});

			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleware();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseAuthorization();

			app.MapControllers();
			
			#endregion

			app.Run();
		}
	}
}