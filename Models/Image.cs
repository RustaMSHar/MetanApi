using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MetanApi.Models
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Int64? length { get; set; }
        public Int32 chunkSize { get; set; }
        public DateOnly uploadDate { get; set; }
        public string md5 { get; set; } = null!;

        [BsonElement("filename")]
        public string? filename { get; set; } = null!;
    }
}
