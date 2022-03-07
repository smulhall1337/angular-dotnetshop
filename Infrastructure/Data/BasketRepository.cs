using Core.Entities;
using Core.Interfaces;
using System.Text.Json;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    /*
     * This will be a different type of repo since we wont be using entity framework to access redis
     */
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            RedisValue data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            /**
             * we'll give baskets a time to live of 30 days
             */
            bool created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!created) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        

        
    }
}
