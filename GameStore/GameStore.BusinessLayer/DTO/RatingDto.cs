namespace GameStore.BusinessLayer.DTO
{
    public class RatingDto
    {
        public RatingDto(string gameId, int votesSum, int votesCount)
        {
            GameId = gameId;
            VotesSum = votesSum;
            VotesCount = votesCount;
        }
        
        public string GameId { get; set; }
        public int VotesSum { get; set; }
        public int VotesCount { get; set; }

        public double Rating
        {
            get
            {
                if (VotesCount == 0)
                {
                    return 0;
                }

                var rating = (double) VotesSum / VotesCount;

                return rating;
            }
        }
    }
}