using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using UnityEngine;

namespace BoardGame
{
    public class Player
    {
        private Stack<Worker> Workers { get; } = new();
        private List<Worker> WorkersRegistry { get; } = new();
        private Stack<Talent> Lineup { get; } = new();

        public int Budget { get; private set; }
        public int Trust { get; private set; }
        public int Views { get; private set; }
        public int AvailableWorkersAmount => Workers.Count;
        public int TotalWorkersAmount => WorkersRegistry.Count;

        private readonly Color _color;

        public int Interest
        {
            get
            {
                var interest = 0;
                foreach (var talent in Lineup)
                {
                    if(talent == null) { continue; }
                    interest += talent.interest;
                }

                return interest;
            }
        }

        public Player()
        {
            _color = Color.HSVToRGB(Random.value, 1, 1);
            Budget = GameDefinitions.InitialBudget;
            for (var i = 0; i < GameDefinitions.MinWorkersPerPlayer; i++)
            {
                AddWorker();
            }
        }

        public bool CanAcquireViews() => Views < GameDefinitions.MaxViews;
        public void AddViews(int views) => Views += views;
        public void ClearViews() => Views = 0;

        public bool CanAcquireWorker() =>
            TotalWorkersAmount < GameDefinitions.MaxWorkersPerPlayer;

        public void AddWorker()
        {
            if (!CanAcquireWorker()) return;
            var worker = new Worker();
            Workers.Push(worker);
            WorkersRegistry.Add(worker);
        }

        public Worker GetWorker()
        {
            return Workers.Count == 0 ? null : Workers.Pop();
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
                Game.Instance.TalentDeck.Withdraw(talent);
            }
            Lineup.Clear();
        }

        public void PayWorkers()
        {
            var total = TotalWorkersAmount * GameDefinitions.CostPerWorker;
            while (total > Budget)
            {
                if (TotalWorkersAmount <= GameDefinitions.MinWorkersPerPlayer)
                {
                    // Working for free.
                    return;
                }
                var workerToDelete = Workers.Pop();
                WorkersRegistry.Remove(workerToDelete);
                total = TotalWorkersAmount * GameDefinitions.CostPerWorker;
            }
            Budget-= total;
        }

        public void IncreaseTrust(int trust)
        {
            Trust += trust;
        }

        public void Receive(uint amount)
        {
            Budget += (int) amount;
        }

        public void Pay(uint cost)
        {
            Budget -= (int) cost;
            if (Budget < 0)
            {
                Debug.LogWarning("Can not pay.");
                Budget = 0;
            }
        }

        public bool MakeTest(int threshold)
        {
            return Random.Range(1, 7) <= threshold;
        }

        public string ColoredText(string text)
        {
            return $"<color=\"{_color.Hex()}\">{text}</color>";
        }

        public override string ToString()
        {
            return ColoredText($"Trust:{Trust} - Budget:{Budget} - Workers:{TotalWorkersAmount} - Lineup{Lineup.Count} - Views:{Views} - Interest {Interest}");
        }
    }
}