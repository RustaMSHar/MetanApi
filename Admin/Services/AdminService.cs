using MetanApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetanApi.Admin.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<Items> _itemsCollection;
        private readonly IGridFSBucket _gridFSBucket;

        public AdminService(IOptions<StoreDatabaseSettings> storeDatabaseSettings)
        {
            var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
            var mongoImageDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.ImageDatabaseName);
            _itemsCollection = mongoDatabase.GetCollection<Items>(storeDatabaseSettings.Value.ItemsCollectionName);

            _gridFSBucket = new GridFSBucket(mongoImageDatabase);

        }
        //ItemsService
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

        //ImagesService
        public async Task<byte[]> GetImageBytesAsync(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                var fileStream = await _gridFSBucket.OpenDownloadStreamAsync(objectId);
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        public async Task<ObjectId> UploadImageAsync(string fileName, Stream stream)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument("filename", fileName)
            };
            return await _gridFSBucket.UploadFromStreamAsync(fileName, stream, options);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteImageAsync(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                try
                {
                    await _gridFSBucket.DeleteAsync(objectId);
                    return (true, null);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
            return (false, "Invalid ObjectId");
        }

        // Добавьте метод для связывания fs.files с Items
        public async Task<bool> LinkImageToItemAsync(string itemId, string imageId)
        {
            if (ObjectId.TryParse(itemId, out ObjectId itemObjectId))
            {
                var filter = Builders<Items>.Filter.Eq(x => x.Id, itemId);
                var update = Builders<Items>.Update.Set(x => x.Picture, imageId); // Используйте поле Picture для хранения _id изображения

                var result = await _itemsCollection.UpdateOneAsync(filter, update);

                return result.IsAcknowledged && result.ModifiedCount > 0;
            }

            return false;
        }



    }
}


/*
    public AdminService(IOptions<StoreDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.ImageDatabaseName);

    }
*/