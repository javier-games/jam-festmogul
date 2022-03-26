using System;
using System.Collections.Generic;
using System.Linq;
using Monogum.BricksBucket.Core;

namespace Game
{
    public class VenueAcquisition 
    {
        private Queue<Venue> _deck = new Queue<Venue>();
        
        private List<Venue> _draw = new List<Venue>();

        private Venue[] _venues = new Venue[GameDefinitions.VenuesOnBoard];

        
        public int AvailableVenues => _venues.Count(t => t != null);


        public VenueAcquisition()
        {
            for (var i = 0; i < GameDefinitions.VenuesDeckCount; i++)
            {
                _draw.Add(Venue.GetRandomVenue());
            }
            ReshuffleDeck();
        }


        public void ReshuffleDeck()
        {
            for (var i = 0; i < _deck.Count; i++)
            {
                _draw.Add(_deck.Dequeue());
            }
            
            _draw.Shuffle();
            _draw.Sort((a, b) => a.Level.CompareTo(b.Level));
            
            for (var i = 0; i < _draw.Count; i++)
            {
                _deck.Enqueue(_draw[i]);
            }
            _draw.Clear();
        }

        
    }
}