using System.Collections.Generic;

namespace BoardGame
{
    public class Player
    {
        public Stack<Worker> Workers { get; } = new();

        private Stack<Talent> Lineup { get; } = new();

        public int Butget;
        public int Trust;
        public int Views;

        public int Interest
        {
            get
            {
                var interest = 0;
                foreach (var talent in Lineup)
                {
                    if(talent == null) { continue; }

                    interest += talent.Interest;
                }

                return interest;
            }
        }

        public Player()
        {
            Butget = GameDefinitions.InitialBudget;
            for (var i = 0; i < GameDefinitions.MinWorkersPerPlayer; i++)
            {
                AddWorker();
            }
        }

        public bool CanAcquireWorker() =>
            Workers.Count < GameDefinitions.MaxWorkersPerPlayer;

        public void AddWorker()
        {
            if (CanAcquireWorker())
            {
                Workers.Push(new Worker());
            }
        }

        public bool CanAcquireTalent() =>
            Lineup.Count < GameDefinitions.TalentPerPlayer;

        public void AcquireTalent(Talent talent) => Lineup.Push(talent);

        public void ReleaseTalent()
        {
            foreach (var talent in Lineup)
            {
                Game.TalentDeck.Return(talent);
            }
            Lineup.Clear();
        }
    }
}