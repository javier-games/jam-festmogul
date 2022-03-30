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

        protected override bool CanExchangePerPlayer(Player player) => true;

        protected override void ExchangePerPlayer(Player player) { }
        
        protected override bool CanExchangePerWorker(Player player) => 
            player.Butget >= GameDefinitions.MarketingCost 
            && player.CanAcquireViews();

        protected override void ExchangePerWorker(Player player)
        {
            player.Butget -= GameDefinitions.MarketingCost;
            player.AddViews(GameDefinitions.MarketingPayback);
        }
    }
}