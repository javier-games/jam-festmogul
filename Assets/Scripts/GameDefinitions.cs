using System;
using Monogum.BricksBucket.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public static class GameDefinitions
    {
        

        public const int MaxPlayers = 6;
        public const int MaxWorkersPerPlayer = 6;

        

        #region Venues
        
        public const int VenuesOnBoard = 3;
        public const int VenuesDeckCount = 20;

        public static readonly Vector2Int VenuesBonusRange = 
            new Vector2Int(1, 3);
        
        public static readonly Venue[] VenuesLevels = 
        {
            new Venue {Cost = 1, Service = 1},
            new Venue {Cost = 2, Service = 1},
            new Venue {Cost = 4, Service = 2},
            new Venue {Cost = 8, Service = 2},
            new Venue {Cost = 16, Service = 3}
        };

        #endregion
        
        
        
        
        public const int AttendantsToMoney = 2;

        public static Season CurrentSeason;

        static GameDefinitions()
        {
            var seasonCount = Enum.GetValues(typeof(Season)).Length;
            CurrentSeason = (Season) Random.Range(0, seasonCount);
        }

        public static void NextSeason()
        {
            var seasonCount = Enum.GetValues(typeof(Season)).Length;
            var currentSeason = (int) CurrentSeason;
            currentSeason.Loop(0, seasonCount);
            CurrentSeason = (Season) currentSeason;
        }

        public static bool MakeTest(int threshold)
        {
            return Random.Range(1, 7) <= threshold;
        }

    }
}