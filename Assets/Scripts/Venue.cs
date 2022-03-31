namespace BoardGame
{
    [System.Serializable]
    public class Venue
    {
        public int level;
        public int cost;
        public int service;
        public int placesQuota;
        public Season seasonForBonus;
        public int interestBonus;

        /*
        public static Venue GetRandomVenue()
        {
            var random = new System.Random();
            var maxLevel = GameDefinitions.VenuesLevels.Length;
            var level = random.Next(maxLevel-1);
            var min = GameDefinitions.VenuesLevels[level];
            var max = GameDefinitions.VenuesLevels[level + 1];
            
            var venue = new Venue
            {
                level = level,
                seasonForBonus = (Season) random.Next(4),
                cost = random.Next(min.cost, max.cost),
                service = random.Next(min.service, max.service),
                placesQuota = random.Next(
                    1, 
                    GameDefinitions.MaxWorkersPerPlayer + 1
                ),
                interestBonus = random.Next(
                    GameDefinitions.VenuesBonusRange.x,
                    GameDefinitions.VenuesBonusRange.y
                )
            };

            return venue;
        }*/
    }
}