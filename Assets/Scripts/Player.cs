using System.Collections.Generic;

namespace Game
{
    public class Player
    {
        private Stack<Worker> Workers { get; }
            = new Stack<Worker>();

        private Stack<Artist> Lineup { get; }
            = new Stack<Artist>();
        
        public int Butget;
        public int Trust;
        public int Interest;
        public int Views;

        public void PlaceWorker(Job job)
        {
            if (Workers.Count == 0) { return; }
            if (!job.HasVacancy(this)) { return; }
            job.Charge(this);
            job.Quota[this] ??= new Stack<Worker>();
            job.Quota[this].Push(Workers.Pop());
        }

        public void RecollectWorkers(Job job)
        {
            if (!job.Quota.ContainsKey(this)) { return; }

            job.Quota[this] ??= new Stack<Worker>();
            foreach (var worker in job.Quota[this])
            {
                job.Payback(this);
                Workers.Push(worker);
            }
            job.Quota[this].Clear();
        }
        
    }
}