using System.Collections.Generic;
using System.Linq;

namespace BoardGame
{
    public abstract class Job
    {
        protected abstract int PlayersQuota { get; }
        protected abstract int PlacesQuota { get;  }
        protected abstract int PlacesPerPlayerQuota { get; }

        private readonly Dictionary<Player, Stack<Worker>> _quota = new();

        private int WorkersCount 
            => _quota.Keys.Sum(player => _quota[player].Count);
        
        private int ActivePlayersCount => _quota.Keys.Count(registeredPlayer => 
            _quota[registeredPlayer].Count > 0);

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

            return !_quota.ContainsKey(player) 
                   || _quota[player].Count < PlacesPerPlayerQuota;
        }

        public int GetWorkers(Player player)
        {
            if (!_quota.ContainsKey(player))
            {
                return 0;
            }

            return _quota[player].Count;
        }
        
        public void PlaceWorker(Player player)
        {
            if (player.WorkersCount == 0) { return; }
            if (!HasVacancy(player)) { return; }

            if (!_quota.ContainsKey(player))
            {
                _quota.Add(player, new Stack<Worker>());
            }
            _quota[player].Push(player.GetWorker());
        }

        public void RecollectWorkers(Player player)
        {
            if (!_quota.ContainsKey(player))
            {
                _quota[player] = new Stack<Worker>();
                return;
            }
            
            if(_quota[player].Count == 0)
            {
                return;
            }

            if (!CanBeChargedPerPlayer(player))
            {
                foreach (var worker in _quota[player])
                {
                    player.ReturnWorker(worker);
                }
                _quota[player].Clear();
                return;
            }
            
            ChargePerPlayer(player);
            foreach (var worker in _quota[player])
            {
                player.ReturnWorker(worker);
                if (!CanBeChargedPerWorker(player)) { continue; }
                ChargePerWorker(player);
                PaybackPerWorker(player);
            }
            PaybackPerPlayer(player);
            
            _quota[player].Clear();
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