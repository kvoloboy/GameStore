namespace GameStore.Core.Models.Identity
{
    public static class Permissions
    {
        public const string ReadDeletedGames = "Read deleted games";
        public const string CreateGame = "Create game";
        public const string UpdateGame = "Update game";
        public const string DeleteGame = "Delete game";
        public const string RateGame = "Rate game";

        public const string CreateGenre = "Create genre";
        public const string UpdateGenre = "Update genre";
        public const string DeleteGenre = "Delete genre";

        public const string CreatePlatform = "Create platform";
        public const string UpdatePlatform = "Update platform";
        public const string DeletePlatform = "Delete platform";

        public const string CreatePublisher = "Create publisher";
        public const string UpdatePublisher = "Update publisher";
        public const string DeletePublisher = "Delete publisher";

        public const string CreateComment = "Create comment";
        public const string DeleteComment = "Delete comment";
        public const string UpdateComment = "Update comment";
        public const string Ban = "Ban user";

        public const string ReadPersonalOrders = "Read personal orders";
        public const string ReadOrders = "Read orders";
        public const string UpdateOrder = "Update order";
        public const string MakeOrder = "Make order";

        public const string CreateRole = "Create role";
        public const string UpdateRole = "Update role";
        public const string DeleteRole = "Delete role";
        public const string SetupRoles = "Setup roles";
        public const string ReadUsers = "Read users";
        public const string ReadRoles = "Read roles";

        public const string ManageImages = "Manage images";
        public const string SubscribeOnNotifications = "Subscribe on notifications";
    }
}