using System.Threading.Tasks;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Models.Interfaces;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Core.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;

namespace GameStore.BusinessLayer.Services.Notification
{
    public class MailSenderService : INotificationSenderService<Order>
    {
        private readonly IEmailNotificationSettings _emailNotificationSettings;

        public MailSenderService(IEmailNotificationSettings emailNotificationSettings)
        {
            _emailNotificationSettings = emailNotificationSettings;
        }

        public async Task NotifyAsync(NotificationContext<Order> context)
        {
            var emailMessage = new MimeMessage
            {
                Subject = "Completed order",
                From =
                {
                    new MailboxAddress(_emailNotificationSettings.Email)
                }
            };

            foreach (var subscriber in context.Subscribers)
            {
                emailMessage.To.Add(new MailboxAddress(subscriber.Email));
            }

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"{emailMessage.Subject}\n{GetOrderDescription(context.Invoker)}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailNotificationSettings.Host, _emailNotificationSettings.Port, true);
            await client.AuthenticateAsync(_emailNotificationSettings.Email, _emailNotificationSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        private static string GetOrderDescription(Order order)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var serialized = JsonConvert.SerializeObject(order, settings);

            return serialized;
        }
    }
}