namespace GameStore.BusinessLayer.Models.Interfaces
{
    public interface IEmailNotificationSettings
    {
        public string Host { get; }
        public string Email { get; }
        public string Password { get; }
        public int Port { get; }
    }
}