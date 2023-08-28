

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MetanApi.Models
{
    public class Items
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("ItemName")]
        [JsonPropertyName("ItemName")]
        public string ItemName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Type { get; set; } = null!;
        public string? Picture { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string[]? OfSize { get; set; }
        public bool IsExist { get; set; }
        public decimal CountExist { get; set; }
        public string? Sex { get; set; }
    }
}
