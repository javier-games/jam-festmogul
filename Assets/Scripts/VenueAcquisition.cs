using UnityEngine;

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
            if (_venue != null) { return; }
            _venue = Game.VenuesDeck.Get();
        }

        protected override bool CanExchangePerWorker(Player player) => false;

        protected override void ExchangePerWorker(Player player){ }

        protected override bool CanExchangePerPlayer(Player player) => 
            _venue != null
            && player.Butget >= _venue.Cost;
        
        protected override void ExchangePerPlayer(Player player)
        {
            if (_venue == null || player == null) { return; }
            
            player.Butget -= _venue.Cost;
            var interest = player.Interest;
            if (Game.CurrentSeason == _venue.SeasonForBonus)
            {
                interest += _venue.InterestBonus;
            }
            
            var attendantsSuccess = 0;
            for (var i = 0; i < player.Views; i++)
            {
                if (player.MakeTest(interest))
                {
                    attendantsSuccess++;
                }
            }

            player.Butget += attendantsSuccess 
                             * GameDefinitions.MoneyPerEachAttendant;

            var trust = 0;
            var totalService = _venue.Service + GetWorkers(player);
            for (var i = 0; i < attendantsSuccess; i++)
            {
                if (player.MakeTest(totalService))
                {
                    trust++;
                }
            }

            player.Trust += trust;
            player.ClearViews();
            player.ReleaseTalent(); 
            Game.VenuesDeck.Return(_venue);
            _venue = null;
        }
    }
}