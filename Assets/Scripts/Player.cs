using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Player
    {
        private Stack<Worker> Workers { get; } = new();

        private Stack<Talent> Lineup { get; } = new();

        public int Butget;
        public int Trust;
        public int Views { get; private set; }
        public int WorkersCount { get; private set; }

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

        public bool CanAcquireViews() => Views < GameDefinitions.MaxViews;
        public void AddViews(int views) => Views += views;
        public void ClearViews() => Views = 0;

        public bool CanAcquireWorker() =>
            Workers.Count < GameDefinitions.MaxWorkersPerPlayer;

        public void AddWorker()
        {
            if (CanAcquireWorker())
            {
                Workers.Push(new Worker());
                WorkersCount++;
            }
        }

        public Worker GetWorker()
        {
            if (Workers.Count == 0)
            {
                return null;
            }
            return Workers.Pop();
        }

        public void ReturnWorker(Worker worker)
        {
            Workers.Push(worker);
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

        public void PayWorkers()
        {
            var total = Workers.Count * GameDefinitions.CostPerWorker;
            if (total > Butget)
            {
                for (var i = 0; i < Butget%GameDefinitions.CostPerWorker; i++)
                {
                    Workers.Pop();
                    WorkersCount--;
                }
            }
            Butget-= Workers.Count * GameDefinitions.CostPerWorker;
        }
        
        public bool MakeTest(int threshold)
        {
            return Random.Range(1, 7) <= threshold;
        }

        public override string ToString()
        {
            return $"Budget:{Butget} - Trust:{Trust} - Views:{Views} - Workers:{Workers.Count}";
        }
    }
}