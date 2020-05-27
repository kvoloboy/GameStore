using FluentAssertions;
using GameStore.BusinessLayer.Services;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class InvoiceServiceTests
    {
        [Test]
        public void CreateInvoiceFile_ReturnsFileWithContent_Always()
        {
            const string id = "1";
            const decimal total = 1;
            var invoiceService = new InvoiceService();

            var file = invoiceService.CreateInvoiceFile(id, id, total);

            file.Data.Should().NotBeNullOrEmpty();
        }
    }
}