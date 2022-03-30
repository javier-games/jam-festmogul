using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using UnityEngine;

namespace BoardGame
{
    public class Player
    {
        private Stack<Worker> Workers { get; } = new();
        private List<Worker> Worker { get; } = new List<Worker>();

        private Stack<Talent> Lineup { get; } = new();

        public int Butget;
        public int Trust;
        public int Views { get; private set; }
        public int WorkersCount => Worker.Count;
        public int CurrentWorkerCount => Workers.Count;

        private Color _color;

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
            _color = Color.HSVToRGB(Random.value, 1, 1);
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
            WorkersCount < GameDefinitions.MaxWorkersPerPlayer;

        public void AddWorker()
        {
            if (CanAcquireWorker())
            {
                var worker = new Worker();
                Workers.Push(worker);
                Worker.Add(worker);
                //WorkersCount++;
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
            var total = WorkersCount * GameDefinitions.CostPerWorker;
            while (total > Butget)
            {
                if (WorkersCount <= GameDefinitions.MinWorkersPerPlayer)
                {
                    // Working for free.
                    return;
                }
                var workerToDelete = Workers.Pop();
                Worker.Remove(workerToDelete);
                total = WorkersCount * GameDefinitions.CostPerWorker;
            }
            Butget-= total;
        }

        
        
        public bool MakeTest(int threshold)
        {
            return Random.Range(1, 7) <= threshold;
        }

        public override string ToString()
        {
            
            return $"<color=\"{_color.Hex()}\">Budget:{Butget} - Trust:{Trust} - Views:{Views} - Workers:{Workers.Count} - Lineup{Lineup.Count}</color>";
        }
    }
}