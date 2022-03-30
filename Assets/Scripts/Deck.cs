using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using Monogum.BricksBucket.Core.Collections;
using UnityEngine;

namespace BoardGame
{
    public class Deck<T> where T : class
    {
        private SerializableQueue _deck = new();
        private readonly List<T> _draw = new();
        private readonly System.Comparison<T> _sortMethod;

        public int CardsAmount => _deck.Count;

        public Deck(
            System.Func<T> factoryMethod,
            int deckLenght,
            System.Comparison<T> sortMethod = null)
        {
            _sortMethod = sortMethod;
            for (var i = 0; i < deckLenght; i++)
            {
                _draw.Add(factoryMethod.Invoke());
            }
            ReshuffleDeck();
        }
        
        public Deck(System.Comparison<T> sortMethod = null)
        {
            _sortMethod = sortMethod;
            _deck = JsonUtility.FromJson<SerializableQueue>(FixedVenues);
            ReshuffleDeck();
        }
        

        public T Get(System.Func<T, bool> condition = null)
        {
            if (_deck.Count == 0)
            {
                return null;
            }
            
            if (condition == null)
            {
                return _deck.Dequeue();
            }

            for (var i = 0; i < _deck.Count; i++)
            {
                var t = _deck.Dequeue();
                if (condition(t))
                {
                    return t;
                }
                _deck.Enqueue(t);
            }

            return null;
        }

        public void Return(T element)
        {
            _draw.Add(element);
        }

        public void ReshuffleDeck()
        {
            for (var i = 0; i < _deck.Count; i++)
            {
                _draw.Add(_deck.Dequeue());
            }
            
            var random = new System.Random();
            for (int i = 0; i < _draw.Count; i++)
            {
                var r = random.Next(0, i);
                _draw.Swap (i, r);
            }
            if (_sortMethod != null)
            {
                _draw.Sort(_sortMethod);
            }
            
            for (var i = 0; i < _draw.Count; i++)
            {
                _deck.Enqueue(_draw[i]);
            }
            _draw.Clear();
        }
    
        private class SerializableQueue : SerializableQueue<T> { }
        
        
        private const string FixedVenues = @"
{
    ""values"":
    [
      {
         ""level"":0,
         ""cost"":1,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":0,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":1,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":1,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":1,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":2,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":1,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":3,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":2,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":0,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":2,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":1,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":2,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":2,
         ""interestBonus"":1
      },
      {
         ""level"":0,
         ""cost"":2,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":3,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":3,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":0,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":3,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":1,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":3,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":2,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":3,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":3,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":4,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":0,
         ""interestBonus"":2
      },
      {
         ""level"":1,
         ""cost"":5,
         ""service"":3,
         ""placesQuota"":1,
         ""seasonForBonus"":1,
         ""interestBonus"":1
      },
      {
         ""level"":1,
         ""cost"":4,
         ""service"":0,
         ""placesQuota"":2,
         ""seasonForBonus"":2,
         ""interestBonus"":2
      },
      {
         ""level"":1,
         ""cost"":5,
         ""service"":3,
         ""placesQuota"":1,
         ""seasonForBonus"":3,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":6,
         ""service"":0,
         ""placesQuota"":1,
         ""seasonForBonus"":0,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":6,
         ""service"":0,
         ""placesQuota"":1,
         ""seasonForBonus"":1,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":6,
         ""service"":0,
         ""placesQuota"":1,
         ""seasonForBonus"":2,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":6,
         ""service"":0,
         ""placesQuota"":1,
         ""seasonForBonus"":3,
         ""interestBonus"":2
      },

      {
         ""level"":2,
         ""cost"":8,
         ""service"":6,
         ""placesQuota"":0,
         ""seasonForBonus"":0,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":7,
         ""service"":3,
         ""placesQuota"":0,
         ""seasonForBonus"":1,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":8,
         ""service"":6,
         ""placesQuota"":0,
         ""seasonForBonus"":2,
         ""interestBonus"":2
      },
      {
         ""level"":2,
         ""cost"":7,
         ""service"":3,
         ""placesQuota"":1,
         ""seasonForBonus"":3,
         ""interestBonus"":2
      }
   ]
}

";
    }

}