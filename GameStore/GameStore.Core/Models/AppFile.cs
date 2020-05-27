namespace GameStore.Core.Models
{
    public class AppFile
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Mime { get; set; }
    }
}