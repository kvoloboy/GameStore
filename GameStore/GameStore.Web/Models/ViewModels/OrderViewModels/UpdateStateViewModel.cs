using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class UpdateStateViewModel
    {
        public string OrderId { get; set; }
        public string State { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
    }
}