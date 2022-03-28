namespace BoardGame
{
    public class InvestmentAcquisition : Job
    {
        protected override int PlayersQuota =>
            GameDefinitions.InvestmentPlayersQuota;
        protected override int PlacesQuota =>
            GameDefinitions.InvestmentPlacesQuota;
        protected override int PlacesPerPlayerQuota =>
            GameDefinitions.InvestmentPlacesPerPlayerQuota;
        
        public override void Prepare() { }

        protected override bool CanBeChargedPerPlayer(Player player) => true;

        protected override bool CanBeChargedPerWorker(Player player) => true;

        protected override void ChargePerPlayer(Player player) { }

        protected override void ChargePerWorker(Player player) { }

        protected override void PaybackPerWorker(Player player)
        {
            player.Butget += GameDefinitions.InvestmentPayback;
        }

        protected override void PaybackPerPlayer(Player player) { }
    }
}