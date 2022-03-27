namespace BoardGame
{
    public class VenueAcquisition : Job
    {
        private Venue _venue;

        protected override int PlayersQuota => 1;

        protected override int PlacesQuota => _venue?.PlacesQuota ?? 0;

        protected override int PlacesPerPlayerQuota => PlacesQuota;

        public override void Prepare()
        {
            _venue = Game.VenuesDeck.Get();
        }

        protected override bool CanBeChargedPerWorker(Player player) => false;

        protected override void ChargePerWorker(Player player){ }
        
        protected override void PaybackPerWorker(Player player){ }
        
        protected override bool CanBeChargedPerPlayer(Player player) => 
            _venue != null
            && player.Butget >= _venue.Cost;
        
        protected override void ChargePerPlayer(Player player)
        {
            if (_venue == null || player == null) { return; }
            player.Butget -= _venue.Cost;
        }

        protected override void PaybackPerPlayer(Player player)
        {
            if (_venue == null || player == null) { return; }

            var interest = player.Interest;
            if (GameDefinitions.CurrentSeason == _venue.SeasonForBonus)
            {
                interest += _venue.InterestBonus;
            }
            
            var attendantsSuccess = 0;
            for (var i = 0; i < player.Views; i++)
            {
                if (GameDefinitions.MakeTest(interest))
                {
                    attendantsSuccess++;
                }
            }

            player.Butget += attendantsSuccess 
                             * GameDefinitions.MoneyPerEachAttendant;

            var trust = 0;
            var totalService = _venue.Service + GetWorkers(player) - 1;
            for (var i = 0; i < attendantsSuccess; i++)
            {
                if (GameDefinitions.MakeTest(totalService))
                {
                    trust++;
                }
            }

            player.Trust += trust;
            player.Views = 0;
            player.ReleaseTalent();
        }
    }
}