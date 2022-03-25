using System.Collections.Generic;

namespace Game
{
    public class Venue
    {
        private int _cost;
        private int _trustRequired;
        private Season _season;
        
        private int _service;
        private int _interestBonus;


        
        private List<Worker> _workers;
        
        
        
        public void MakeEvent(Player player)
        {
            var attendants = 0;
            for (var i = 0; i < player.Views; i++)
            {
                if (Utils.MakeTest(player.Interest))
                {
                    attendants++;
                }
            }

            player.Butget += attendants * Utils.AttendantsToMoney;

            var trust = 0;
            for (var i = 0; i < attendants; i++)
            {
                if (Utils.MakeTest(_service))
                {
                    trust++;
                }
            }

            player.Trust += trust;
        }
    }
}