using MetanApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

public class ImageService
{
    private readonly IMongoCollection<BsonDocument> _filesCollection;
    private readonly IGridFSBucket _gridFSBucket;

    public ImageService(StoreDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.ImageDatabaseName);
        _gridFSBucket = new GridFSBucket(database);
        _filesCollection = database.GetCollection<BsonDocument>("fs.files");
    }

    public async Task<ObjectId> UploadImageAsync(Stream stream, string filename)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument("filename", filename)
        };

        ObjectId objectId = await _gridFSBucket.UploadFromStreamAsync(filename, stream, options);
        return objectId;
    }

    public async Task<Stream> GetImageStreamAsync(ObjectId objectId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
        var fileDocument = await _filesCollection.Find(filter).FirstOrDefaultAsync();

        if (fileDocument != null)
        {
            var filename = fileDocument.GetValue("filename").AsString;
            return await _gridFSBucket.OpenDownloadStreamByNameAsync(filename);
        }
        else
        {
            return null; // Файл не найден
        }
    }
}
