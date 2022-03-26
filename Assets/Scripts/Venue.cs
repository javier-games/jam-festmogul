

using UnityEngine;

namespace Game
{
    public class Venue : Job
    {
        public int Level;
        public int Cost;
        public Season SeasonForBonus;
        public int Service;
        public int InterestBonus;


        protected override int PlayersQuota => 1;

        protected override int PlacesQuota { get; } =
            Random.Range(1, GameDefinitions.MaxWorkersPerPlayer + 1);

        protected override int PlacesPerPlayerQuota => PlacesQuota;

        public override void Prepare() { }

        public override bool CanBeEmployee(Player player)
        {
            return base.CanBeEmployee(player) && player.Butget >= Cost;
        }

        protected override void ChargePerWorker(Player player){ }
        
        protected override void ChargePerPlayer(Player player)
        {
            player.Butget -= Cost;
        }
        
        protected override void PaybackPerWorker(Player player){ }
        
        protected override void PaybackPerPlayer(Player player)
        {
            if (GameDefinitions.CurrentSeason == SeasonForBonus)
            {
                player.Interest += InterestBonus;
            }
            
            var attendants = 0;
            for (var i = 0; i < player.Views; i++)
            {
                if (GameDefinitions.MakeTest(player.Interest))
                {
                    attendants++;
                }
            }

            player.Butget += attendants * GameDefinitions.AttendantsToMoney;

            var trust = 0;
            for (var i = 0; i < attendants; i++)
            {
                if (GameDefinitions.MakeTest(Service + GetWorkers(player) - 1))
                {
                    trust++;
                }
            }

            player.Trust += trust;
        }
        
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
                InterestBonus = Random.Range(
                    GameDefinitions.VenuesBonusRange.x,
                    GameDefinitions.VenuesBonusRange.y
                )
            };

            return venue;
        }
    }
}