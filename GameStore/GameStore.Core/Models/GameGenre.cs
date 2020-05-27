namespace GameStore.Core.Models
{
    public class GameGenre
    {
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }

        public string GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}