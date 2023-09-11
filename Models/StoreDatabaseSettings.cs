namespace MetanApi.Models
{
    public class StoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ItemsCollectionName { get; set; } = null!;
        public string ImageDatabaseName { get; set; } = null!;
        public string ImageCollectionName { get; set; } = null!;

    }
}
