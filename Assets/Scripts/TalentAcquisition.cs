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
            Talent = Game.TalentDeck.Get(talent => talent.Level == _level);
        }

        protected override bool CanBeChargedPerWorker(Player player) => false;
        
        protected override void ChargePerWorker(Player player){ }
        
        protected override void PaybackPerWorker(Player player){ }
        
        protected override bool CanBeChargedPerPlayer(Player player) => 
            Talent != null
            && player.Butget >= Talent.Cost
            && player.CanAcquireTalent();
        
        protected override void ChargePerPlayer(Player player)
        {
            if (Talent == null || player == null) { return; }
            player.Butget -= Talent.Cost;
        }

        protected override void PaybackPerPlayer(Player player)
        {
            if (Talent == null || player == null) { return; }
            player.AcquireTalent(Talent);
            Talent = null;
        }
    }
}