using GameStore.BusinessLayer.Models.Interfaces;

namespace GameStore.BusinessLayer.Models
{
    public class EmailNotificationSettings : IEmailNotificationSettings
    {
        public EmailNotificationSettings(string host, string email, string password, int port)
        {
            Host = host;
            Email = email;
            Password = password;
            Port = port;
        }

        public string Host { get; }
        public string Email { get; }
        public string Password { get; }
        public int Port { get; }
    }
}