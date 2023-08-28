

namespace MetanApi.Models
{
    public class StoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ItemsCollectionName { get; set; } = null!;
    }
}
