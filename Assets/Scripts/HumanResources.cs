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

        protected override bool CanBeChargedPerPlayer(Player player) => true;

        protected override bool CanBeChargedPerWorker(Player player) => 
            player.CanAcquireWorker();

        protected override void ChargePerPlayer(Player player) { }

        protected override void ChargePerWorker(Player player) { }

        protected override void PaybackPerWorker(Player player)
        {
            player.AddWorker();
        }

        protected override void PaybackPerPlayer(Player player) { }
    }
}