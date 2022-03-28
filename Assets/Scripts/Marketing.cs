namespace BoardGame
{
    public class Marketing : Job
    {
        protected override int PlayersQuota => 
            GameDefinitions.MarketingPlayersQuota;
        protected override int PlacesQuota => 
            GameDefinitions.MarketingPlacesQuota;
        protected override int PlacesPerPlayerQuota =>
            GameDefinitions.MarketingPlacesPerPlayerQuota;
        
        public override void Prepare() { }

        protected override bool CanBeChargedPerPlayer(Player player) => true;

        protected override void ChargePerPlayer(Player player) { }
        
        protected override void PaybackPerPlayer(Player player) { }
        
        protected override bool CanBeChargedPerWorker(Player player) => 
            player.Butget >= GameDefinitions.MarketingCost 
            && player.CanAcquireViews();

        protected override void ChargePerWorker(Player player)
        {
            player.Butget -= GameDefinitions.MarketingCost;
        }

        protected override void PaybackPerWorker(Player player)
        {
            player.AddViews( GameDefinitions.MarketingPayback);
        }

    }
}