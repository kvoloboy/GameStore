using System;
using System.Collections.Generic;

namespace GameStore.Core.Models
{
    public class GameRoot
    {
        public GameRoot()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Key { get; set; }
        public GameDetails Details { get; set; }
        public string PublisherEntityId { get; set; }
        public Publisher Publisher { get; set; }
        public Visit Visit { get; set; } = new Visit();
        public ICollection<Comment> Comments { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<GameLocalization> Localizations { get; set; } = new List<GameLocalization>();
        public ICollection<Rating> GameRatings { get; set; } = new List<Rating>();
        public ICollection<GameImage> GameImages { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is GameRoot gameRoot))
            {
                return false;
            }

            var areEqualsDetails = Key == gameRoot.Key && Equals(Details, gameRoot.Details);
            
            return areEqualsDetails;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(Id);
            hashCode.Add(Key);
            hashCode.Add(Details);
            hashCode.Add(Visit);
            hashCode.Add(Comments);
            hashCode.Add(GamePlatforms);
            hashCode.Add(GameGenres);

            return hashCode.ToHashCode();
        }
    }
}