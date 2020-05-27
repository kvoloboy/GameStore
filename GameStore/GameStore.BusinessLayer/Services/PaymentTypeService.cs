using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IAsyncRepository<PaymentType> _paymentRepository;

        public PaymentTypeService(IAsyncRepository<PaymentType> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentType>> GetAllAsync()
        {
            var payments = await _paymentRepository.FindAllAsync();

            return payments;
        }
    }
}