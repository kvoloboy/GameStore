using System;
using System.Text;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class InvoiceService : IInvoiceService
    {
        public AppFile CreateInvoiceFile(string orderId, string userId, decimal total)
        {
            const string contentType = "text/plain";
            var fileName = $"Game store{orderId}.txt";
            var markup = GetInvoiceDescription(orderId, userId, total);
            var fileData = Encoding.UTF8.GetBytes(markup);
            var file = new AppFile
            {
                Name = fileName,
                Data = fileData,
                Mime = contentType
            };

            return file;
        }

        private static string GetInvoiceDescription(string orderId, string userId, decimal total)
        {
            var currentDate = DateTime.UtcNow;
            var sb = new StringBuilder();
            sb.Append($"|INVOICE|{Environment.NewLine}");
            sb.Append($"Invoice creation time UTC: {currentDate}.{Environment.NewLine}");
            sb.Append($"User id: {userId}.{Environment.NewLine}");
            sb.Append($"Order id: {orderId}.{Environment.NewLine}");
            sb.Append($"Total price = {total:c2}");

            return sb.ToString();
        }
    }
}