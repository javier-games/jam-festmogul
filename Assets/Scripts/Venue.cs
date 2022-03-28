using System;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public class Venue
    {
        public int Level;
        public int Cost;
        public Season SeasonForBonus;
        public int Service;
        public int InterestBonus;
        public int PlacesQuota;

        public static Venue GetRandomVenue()
        {
            var random = new System.Random();
            var maxLevel = GameDefinitions.VenuesLevels.Length;
            var level = random.Next(maxLevel-1);
            var min = GameDefinitions.VenuesLevels[level];
            var max = GameDefinitions.VenuesLevels[level + 1];
            
            var venue = new Venue
            {
                Level = level,
                SeasonForBonus = (Season) random.Next(4),
                Cost = random.Next(min.Cost, max.Cost),
                Service = random.Next(min.Service, max.Service),
                PlacesQuota = random.Next(
                    1, 
                    GameDefinitions.MaxWorkersPerPlayer + 1
                ),
                InterestBonus = random.Next(
                    GameDefinitions.VenuesBonusRange.x,
                    GameDefinitions.VenuesBonusRange.y
                )
            };

            return venue;
        }
    }
}