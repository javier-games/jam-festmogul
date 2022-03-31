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

        public override int PlacesPerPlayerQuota => PlacesQuota;

        public override void Prepare()
        {
            if (_talent != null) return;
            if (Game.Instance.TalentDeck.CardsAmount <= 0)
            {
                Game.Instance.TalentDeck.ReshuffleDeck();
            }
            _talent = Game.Instance.TalentDeck.Draw(talent => talent.level == _level);
        }

        public override bool HasVacancy(Player player)
        {
            return base.HasVacancy(player) 
                   // TODO: Rule added for computer.
                   && player.CanAcquireTalent();
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