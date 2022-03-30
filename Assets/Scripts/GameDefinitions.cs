using UnityEngine;

namespace BoardGame
{
    public static class GameDefinitions
    {

        public const int MinPlayers = 4;
        public const int MaxPlayers = 4;
        
        public const int MinWorkersPerPlayer = 1;
        public const int MaxWorkersPerPlayer = 6;
        
        public const int InitialBudget = 12;
        public const int MaxViews = 6;

        #region Venues

        public const int VenuesSlots = 3;
        
        public const int VenuesDeckCount = 20;
        public static readonly Vector2Int VenuesBonusRange = new(1, 3);
        public static readonly Venue[] VenuesLevels = 
        {
            new() {cost = 1, service = 1},
            new() {cost = 2, service = 1},
            new() {cost = 4, service = 2},
            new() {cost = 8, service = 2},
            new() {cost = 16, service = 3}
        };

        public const int ServicePerWorker = 3;
        
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
            new () {cost = 3, interest = 1},
            new () {cost = 6, interest = 2},
            new () {cost = 9, interest = 3}
        };

        #endregion

        #region Marketing

        public const int MarketingPlayersQuota = 2;
        public const int MarketingPlacesQuota = 2;
        public const int MarketingPlacesPerPlayerQuota = 1;
        public const int MarketingCost = 2;
        public const int MarketingPayback = 1;

        #endregion

        #region Inverstments

        public const int InvestmentPlayersQuota  = MaxPlayers;
        public const int InvestmentPlacesQuota  = MaxWorkersPerPlayer * MaxPlayers;
        public const int InvestmentPlacesPerPlayerQuota = MaxWorkersPerPlayer;
        public const int InvestmentPayback = 5;

        #endregion

        #region Human Resources
        
        public const int HumanResourcesPlayersQuota  = 1;
        public const int HumanResourcesPlacesQuota  = 1;
        public const int HumanResourcesPlacesPerPlayerQuota = 1;

        #endregion

        public const int CostPerWorker = 1;
        public const int MoneyPerEachAttendant = 2;
    }
}