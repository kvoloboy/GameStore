namespace GameStore.Infrastructure.Logging.Models
{
    public abstract class BaseLogEntry
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string EntityType { get; set; }
        public string Operation { get; set; }
        public string Version { get; set; }
    }
}