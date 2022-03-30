﻿namespace BoardGame
{
    public class VenueAcquisition : Job
    {
        private Venue _venue;

        protected override int PlayersQuota => 1;

        protected override int PlacesQuota => _venue?.placesQuota ?? 0;

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
            && player.Budget >= _venue.cost;
        
        protected override void ExchangePerPlayer(Player player)
        {
            if (_venue == null || player == null) { return; }
            
            player.Pay((uint) _venue.cost);
            var interest = player.Interest;
            if (Game.CurrentSeason == _venue.seasonForBonus)
            {
                interest += _venue.interestBonus;
            }
            
            var attendantsSuccess = 0;
            for (var i = 0; i < player.Views; i++)
            {
                if (player.MakeTest(interest))
                {
                    attendantsSuccess++;
                }
            }

            player.Receive( (uint) attendantsSuccess * GameDefinitions.MoneyPerEachAttendant);

            var trust = 0;
            var totalService = _venue.service + (GetWorkers(player) * GameDefinitions.ServicePerWorker);
            for (var i = 0; i < attendantsSuccess; i++)
            {
                if (player.MakeTest(totalService))
                {
                    trust++;
                }
            }

            player.IncreaseTrust(trust);
            player.ClearViews();
            player.ReleaseTalent(); 
            Game.VenuesDeck.Return(_venue);
            _venue = null;
        }
    }
}