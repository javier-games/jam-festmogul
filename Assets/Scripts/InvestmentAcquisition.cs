using UnityEngine;

namespace BoardGame
{
    public class InvestmentAcquisition : Job
    {
        protected override int PlayersQuota =>
            GameDefinitions.InvestmentPlayersQuota;
        protected override int PlacesQuota =>
            GameDefinitions.InvestmentPlacesQuota;
        public override int PlacesPerPlayerQuota =>
            GameDefinitions.InvestmentPlacesPerPlayerQuota;
        
        public override void Prepare() { }

        protected override bool CanExchangePerPlayer(Player player) => true;

        protected override bool CanExchangePerWorker(Player player) => true;

        protected override void ExchangePerPlayer(Player player) { }

        protected override void ExchangePerWorker(Player player) => 
            player.Receive(GameDefinitions.InvestmentPayback);
    }
}