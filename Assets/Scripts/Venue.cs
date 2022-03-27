using UnityEngine;

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
            var maxLevel = GameDefinitions.VenuesLevels.Length;
            var level = Random.Range(0, maxLevel);
            var min = GameDefinitions.VenuesLevels[level];
            var max = GameDefinitions.VenuesLevels[level + 1];
            
            var venue = new Venue
            {
                Level = level,
                SeasonForBonus = (Season) Random.Range(0, 4),
                Cost = Random.Range(min.Cost, max.Cost),
                Service = Random.Range(min.Service, max.Service),
                PlacesQuota = Random.Range(
                    1, 
                    GameDefinitions.MaxWorkersPerPlayer + 1
                ),
                InterestBonus = Random.Range(
                    GameDefinitions.VenuesBonusRange.x,
                    GameDefinitions.VenuesBonusRange.y
                )
            };

            return venue;
        }
    }
}