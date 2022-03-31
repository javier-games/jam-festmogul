using UnityEngine;

namespace BoardGame
{
    public class HumanResources : Job
    {
        protected override int PlayersQuota =>
            GameDefinitions.HumanResourcesPlayersQuota;
        protected override int PlacesQuota =>
            GameDefinitions.HumanResourcesPlacesQuota;
        protected override int PlacesPerPlayerQuota =>
            GameDefinitions.HumanResourcesPlacesPerPlayerQuota;
        public override void Prepare() { }

        protected override bool CanExchangePerPlayer(Player player) => true;

        protected override bool CanExchangePerWorker(Player player) => 
            player.CanAcquireWorker();

        protected override void ExchangePerPlayer(Player player) { }

        protected override void ExchangePerWorker(Player player)
        {
            Debug.Log("Should be added");
            player.AddWorker();
        }
    }
}