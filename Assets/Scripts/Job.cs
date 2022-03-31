using System.Collections.Generic;
using System.Linq;

namespace BoardGame
{
    public abstract class Job
    {
        protected abstract int PlayersQuota { get; }
        protected abstract int PlacesQuota { get;  }
        public abstract int PlacesPerPlayerQuota { get; }

        private readonly Dictionary<Player, Stack<Worker>> _quota = new();

        private int WorkersCount => _quota.Keys.Sum(key => _quota[key].Count);
        
        private int PlayersCount => _quota.Keys.Count(key => _quota[key].Count > 0);

        public virtual bool HasVacancy(Player player)
        {
            if (WorkersCount >= PlacesQuota) { return false; }
            if (PlayersCount >= PlayersQuota) { return false; }
            return GetWorkers(player) < PlacesPerPlayerQuota;
        }

        public int GetWorkers(Player player)
        {
            return !_quota.ContainsKey(player) ? 0 : _quota[player].Count;
        }
        
        public void PlaceWorker(Player player)
        {
            if (player.AvailableWorkersAmount == 0) { return; }
            if (!HasVacancy(player)) { return; }

            if (!_quota.ContainsKey(player))
            {
                _quota.Add(player, new Stack<Worker>());
            }

            var worker = player.GetWorker();
            if (worker == null) { return; }
            _quota[player].Push(worker);
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

            if (!CanExchangePerPlayer(player))
            {
                foreach (var worker in _quota[player])
                {
                    player.ReturnWorker(worker);
                }
                _quota[player].Clear();
                return;
            }
            
            ExchangePerPlayer(player);
            foreach (var worker in _quota[player])
            {
                player.ReturnWorker(worker);
                if (!CanExchangePerWorker(player)) { continue; }
                ExchangePerWorker(player);
            }
            
            _quota[player].Clear();
        }
        
        public abstract void Prepare();
        protected abstract bool CanExchangePerPlayer(Player player);
        protected abstract bool CanExchangePerWorker(Player player);
        protected abstract void ExchangePerPlayer(Player player);
        protected abstract void ExchangePerWorker(Player player);
    }
}