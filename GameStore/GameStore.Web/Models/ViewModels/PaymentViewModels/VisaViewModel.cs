using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Helpers.CustomDataAnnotations;

namespace GameStore.Web.Models.ViewModels.PaymentViewModels
{
    public class VisaViewModel
    {
        [Required]
        public string OrderId { get; set; }

        [Required(ErrorMessage = "CardHolderName")]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "CardNumber")]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        [CurrentDate(ErrorMessage = "ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [Cvv(ErrorMessage = "ThreeSymbolsCode")]
        public string CvCode { get; set; }
    }
}