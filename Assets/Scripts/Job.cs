using System.Collections.Generic;
using System.Linq;

namespace BoardGame
{
    public abstract class Job
    {
        protected abstract int PlayersQuota { get; }
        protected abstract int PlacesQuota { get;  }
        protected abstract int PlacesPerPlayerQuota { get; }

        public Dictionary<Player, Stack<Worker>> Quota = new();

        private int WorkersCount 
            => Quota.Keys.Sum(player => Quota[player].Count);
        
        private int ActivePlayersCount => Quota.Keys.Count(registeredPlayer => 
            Quota[registeredPlayer].Count > 0);

        public bool HasVacancy(Player player)
        {
            if (WorkersCount >= PlacesQuota)
            {
                return false;
            }
            
            if (ActivePlayersCount >= PlayersQuota)
            {
                return false;
            }

            return !Quota.ContainsKey(player) 
                   || Quota[player].Count < PlacesPerPlayerQuota;
        }

        public int GetWorkers(Player player)
        {
            if (!Quota.ContainsKey(player))
            {
                return 0;
            }

            return Quota[player].Count;
        }
        
        public void PlaceWorker(Player player)
        {
            if (player.Workers.Count == 0) { return; }
            if (!HasVacancy(player)) { return; }

            Quota[player] ??= new Stack<Worker>();
            Quota[player].Push(player.Workers.Pop());
        }

        public void RecollectWorkers(Player player)
        {
            if (!Quota.ContainsKey(player))
            {
                Quota[player] = new Stack<Worker>();
                return;
            }
            
            if(Quota[player].Count == 0)
            {
                return;
            }

            if (!CanBeChargedPerPlayer(player))
            {
                foreach (var worker in Quota[player])
                {
                    player.Workers.Push(worker);
                }
                Quota[player].Clear();
                return;
            }
            
            ChargePerPlayer(player);
            foreach (var worker in Quota[player])
            {
                player.Workers.Push(worker);
                if (!CanBeChargedPerWorker(player)) { continue; }
                ChargePerWorker(player);
                PaybackPerWorker(player);
            }
            PaybackPerPlayer(player);
            
            Quota[player].Clear();
        }
        
        public abstract void Prepare();
        protected abstract bool CanBeChargedPerPlayer(Player player);
        protected abstract bool CanBeChargedPerWorker(Player player);
        protected abstract void ChargePerPlayer(Player player);
        protected abstract void ChargePerWorker(Player player);
        protected abstract void PaybackPerWorker(Player player);
        protected abstract void PaybackPerPlayer(Player player);
    }
}