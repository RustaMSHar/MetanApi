using MetanApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MetanApi.Services
{
    public class ItemsService
    {
        private readonly IMongoCollection<Items> _itemsCollection;

        public ItemsService(
        IOptions<StoreDatabaseSettings> storeDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                storeDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                storeDatabaseSettings.Value.DatabaseName);

            _itemsCollection = mongoDatabase.GetCollection<Items>(
                storeDatabaseSettings.Value.ItemsCollectionName);
        }

        public async Task<List<Items>> GetAsync() =>
            await _itemsCollection.Find(_ => true).ToListAsync();

        public async Task<Items?> GetAsync(string id) =>
            await _itemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Items>> GetAsync(FilterDefinition<Items> filter) =>
          await _itemsCollection.Find(filter).ToListAsync();

        public async Task CreateAsync(Items newItem) =>
            await _itemsCollection.InsertOneAsync(newItem);

        public async Task UpdateAsync(string id, Items updatedItem) =>
            await _itemsCollection.ReplaceOneAsync(x => x.Id == id, updatedItem);

        public async Task RemoveAsync(string id) =>
            await _itemsCollection.DeleteOneAsync(x => x.Id == id);

    }
}
