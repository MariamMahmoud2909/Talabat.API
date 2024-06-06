using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.Core.Services.Contract;

namespace Talabat.Service.CasheService
{
	public class ResponseCasheService : IResponseCashService
	{
		private readonly IDatabase _database;

        public ResponseCasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CasheResponseAsync(string key, object Response, TimeSpan TimeToLive)
		{
			if (Response is null) return;

			var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var serializedResponse = JsonSerializer.Serialize(Response , serializeOptions);

			await _database.StringSetAsync(key , serializedResponse , TimeToLive);
		}

		public async Task<string?> GetCashedResponseAsync(string key)
		{
			var response = await _database.StringGetAsync(key);

			if (response.IsNullOrEmpty) return null;
			
			return response;
		}
	}
}
