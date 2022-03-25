using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public abstract class Job
    {
        public int PlayersQuota;
        public int PlacesQuota;
        public int PlacesPerPlayerQuota;

        public Dictionary<Player, Stack<Worker>> Quota 
            = new Dictionary<Player, Stack<Worker>>();

        private int WorkersCount 
            => Quota.Keys.Sum(player => Quota[player].Count);

        public virtual bool HasVacancy(Player player) => 
            WorkersCount < PlacesQuota && (
                Quota.ContainsKey(player) 
                    ? Quota[player].Count < PlacesPerPlayerQuota
                    : Quota.Count < PlayersQuota
                );


        public abstract void Charge(Player player);
        public abstract void Payback(Player player);
    }
}