using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public abstract class Job
    {
        protected abstract int PlayersQuota { get; }
        protected abstract int PlacesQuota { get;  }
        protected abstract int PlacesPerPlayerQuota { get; }

        public Dictionary<Player, Stack<Worker>> Quota 
            = new Dictionary<Player, Stack<Worker>>();

        private int WorkersCount 
            => Quota.Keys.Sum(player => Quota[player].Count);

        public virtual bool CanBeEmployee(Player player) => 
            WorkersCount < PlacesQuota && (
                Quota.ContainsKey(player) 
                    ? Quota[player].Count < PlacesPerPlayerQuota
                    : Quota.Count < PlayersQuota
                );

        public int GetWorkers(Player player)
        {
            if (!Quota.ContainsKey(player))
            {
                return 0;
            }

            return Quota[player].Count;
        }
        
        public void PlaceWorkers(Player player, int workersToPlace)
        {
            if (player.Workers.Count == 0) { return; }
            if (!CanBeEmployee(player)) { return; }
            if (workersToPlace > player.Workers.Count)
            {
                workersToPlace = player.Workers.Count;
            }

            Quota[player] ??= new Stack<Worker>(); 
            for (var i = 0; i < workersToPlace; i++)
            {
                ChargePerWorker(player); 
                Quota[player].Push(player.Workers.Pop());
            }
            ChargePerPlayer(player);
        }

        public void RecollectWorkers(Player player)
        {
            if (!Quota.ContainsKey(player)) { return; }

            Quota[player] ??= new Stack<Worker>();
            foreach (var worker in Quota[player])
            {
                player.Workers.Push(worker);
                PaybackPerWorker(player);
            }
            Quota[player].Clear();
            PaybackPerPlayer(player);
        }
        
        public abstract void Prepare();
        protected abstract void ChargePerPlayer(Player player);
        protected abstract void ChargePerWorker(Player player);
        protected abstract void PaybackPerWorker(Player player);
        protected abstract void PaybackPerPlayer(Player player);
    }
}