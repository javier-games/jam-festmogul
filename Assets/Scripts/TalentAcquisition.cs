namespace BoardGame
{
    public class TalentAcquisition : Job
    {
        private Talent Talent;
        private readonly int _level;

        public TalentAcquisition(int level)
        {
            _level = level;
        }

        protected override int PlayersQuota => 1;

        protected override int PlacesQuota => Talent != null ? 1: 0;

        protected override int PlacesPerPlayerQuota => PlacesQuota;

        public override void Prepare()
        {
            if (Talent != null) return;
            Talent = Game.TalentDeck.Get(talent => talent.Level == _level);
        }

        protected override bool CanExchangePerWorker(Player player) => false;
        
        protected override void ExchangePerWorker(Player player){ }

        protected override bool CanExchangePerPlayer(Player player) => 
            Talent != null
            && player.Butget >= Talent.Cost
            && player.CanAcquireTalent();
        
        protected override void ExchangePerPlayer(Player player)
        {
            if (Talent == null || player == null) { return; }
            player.Butget -= Talent.Cost;
            player.AcquireTalent(Talent);
            Talent = null;
        }
    }
}