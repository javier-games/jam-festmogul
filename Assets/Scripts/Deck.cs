using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using Monogum.BricksBucket.Core.Collections;
using UnityEngine;

namespace BoardGame
{
    public class Deck<T> where T : class
    {
        private readonly SerializableQueue _availableDeck = new();
        private readonly List<T> _discardedDeck = new();
        private readonly System.Comparison<T> _sortMethod;

        public int CardsAmount => _availableDeck.Count;

        public Deck(System.Comparison<T> sortMethod = null)
        {
            _sortMethod = sortMethod;
        }
        
        public T Draw(System.Func<T, bool> condition = null)
        {
            if (_availableDeck.Count == 0)
            {
                return null;
            }
            
            if (condition == null)
            {
                return _availableDeck.Dequeue();
            }

            for (var i = 0; i < _availableDeck.Count; i++)
            {
                var t = _availableDeck.Dequeue();
                if (condition(t))
                {
                    return t;
                }
                _availableDeck.Enqueue(t);
            }

            return null;
        }

        public void Withdraw(T element)
        {
            if (element == null) { return; }
            _discardedDeck.Add(element);
        }

        public void AddCardsFromFile(TextAsset asset)
        {
            var text = asset ? asset.text : string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning("Text null.");
                return;
            }
            
            var deck = JsonUtility.FromJson<SerializableQueue>(text);
            if (deck == null)
            {
                Debug.LogWarning("Deck empty.");
                return;
            }

            while (deck.Count > 0)
            {
                _availableDeck.Enqueue(deck.Dequeue());
            }
            
            ReshuffleDeck();
        }

        public void AddCardsFromFactory(System.Func<T> factoryMethod, int deckLenght)
        {
            for (var i = 0; i < deckLenght; i++)
            {
                _availableDeck.Enqueue(factoryMethod.Invoke());
            }
            ReshuffleDeck();
            Debug.Log(JsonUtility.ToJson(_availableDeck, true));
        }

        public void ReshuffleDeck()
        {
            while (_availableDeck.Count > 0)
            {
                _discardedDeck.Add(_availableDeck.Dequeue());
            }
            
            var random = new System.Random();
            for (int i = 0; i < _discardedDeck.Count; i++)
            {
                var r = random.Next(0, i);
                _discardedDeck.Swap (i, r);
            }
            if (_sortMethod != null)
            {
                Debug.Log("sorted");
                Debug.Log(_discardedDeck.Count);
                _discardedDeck.Sort(_sortMethod);
            }
            
            for (var i = 0; i < _discardedDeck.Count; i++)
            {
                _availableDeck.Enqueue(_discardedDeck[i]);
            }
            _discardedDeck.Clear();
        }
    
        private class SerializableQueue : SerializableQueue<T> { }
    }

}