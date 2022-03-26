using System.Collections.Generic;

namespace Game
{
    public class Player
    {
        public Stack<Worker> Workers { get; }
            = new Stack<Worker>();

        private Stack<Artist> Lineup { get; }
            = new Stack<Artist>();
        
        public int Butget;
        public int Trust;
        public int Interest;
        public int Views;

        
        
    }
}