using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.UserViewModels
{
    public class SubscribeOnNotificationsViewModel
    {
        public IEnumerable<ListItem> NotificationMethods { get; set; }
        public IEnumerable<string> SelectedNotifications { get; set; }
    }
}