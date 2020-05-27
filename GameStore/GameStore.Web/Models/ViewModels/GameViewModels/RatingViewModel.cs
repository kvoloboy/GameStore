using System;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class RatingViewModel
    {
        public static int MaxStarsValue => 5;

        public int VotesCount { get; set; }
        public int VotesSum { get; set; }
        public string GameId { get; set; }

        public int StarsCount => (int) Math.Round(Rating, MidpointRounding.AwayFromZero);

        public double Rating { get; set; }
    }
}