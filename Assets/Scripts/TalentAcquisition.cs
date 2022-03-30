namespace BoardGame
{
    public class TalentAcquisition : Job
    {
        private Talent _talent;
        private readonly int _level;

        public TalentAcquisition(int level)
        {
            _level = level;
        }

        protected override int PlayersQuota => 1;

        protected override int PlacesQuota => _talent != null ? 1: 0;

        protected override int PlacesPerPlayerQuota => PlacesQuota;

        public override void Prepare()
        {
            if (_talent != null) return;
            _talent = Game.TalentDeck.Get(talent => talent.level == _level);
        }

        protected override bool CanExchangePerWorker(Player player) => false;
        
        protected override void ExchangePerWorker(Player player){ }

        protected override bool CanExchangePerPlayer(Player player) => 
            _talent != null
            && player.Budget >= _talent.cost
            && player.CanAcquireTalent();
        
        protected override void ExchangePerPlayer(Player player)
        {
            if (_talent == null || player == null) { return; }
            player.Pay((uint) _talent.cost);
            player.AcquireTalent(_talent);
            _talent = null;
        }
    }
}