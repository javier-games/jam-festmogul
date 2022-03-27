using System;
using Monogum.BricksBucket.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public static class GameDefinitions
    {

        public const int MinPlayers = 2;
        public const int MaxPlayers = 6;
        public const int MinWorkersPerPlayer = 1;
        public const int MaxWorkersPerPlayer = 6;

        public const int InitialBudget = 12;

        #region Venues

        public const int VenuesSlots = 3;
        
        public const int VenuesDeckCount = 20;

        public static readonly Vector2Int VenuesBonusRange = 
            new(1, 3);
        
        public static readonly Venue[] VenuesLevels = 
        {
            new() {Cost = 1, Service = 1},
            new() {Cost = 2, Service = 1},
            new() {Cost = 4, Service = 2},
            new() {Cost = 8, Service = 2},
            new() {Cost = 16, Service = 3}
        };

        #endregion

        #region Talent

        public static readonly int[] TalentSlots =
        {
            2, 
            3, 
            2
        };
        
        public const int TalentDeckCount = 20;
        public const int TalentPerPlayer = 3;
        
        public static readonly Talent[] TalentLevels =
        {
            new () {Cost = 3, Interest = 1},
            new () {Cost = 6, Interest = 2},
            new () {Cost = 9, Interest = 3}
        };

        #endregion
        
        
        
        
        public const int MoneyPerEachAttendant = 2;

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