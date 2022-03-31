using UnityEngine;

namespace BoardGame
{
    public static class GameDefinitions
    {

        public const int MinPlayers = 4;
        public const int MaxPlayers = 4;
        
        public const int MinWorkersPerPlayer = 2;
        public const int MaxWorkersPerPlayer = 6;
        
        public const int InitialBudget = 50;
        public const int MaxViews = 6;

        #region Venues

        public const int VenuesSlots = 3;
        public const int ServicePerWorker = 3;
        
        #endregion

        #region Talent

        public static readonly int[] TalentSlots = {2, 3, 2};
        public const int TalentPerPlayer = 3;

        #endregion

        #region Marketing

        public const int MarketingPlayersQuota = 2;
        public const int MarketingPlacesQuota = 2;
        public const int MarketingPlacesPerPlayerQuota = 1;
        public const int MarketingCost = 5;
        public const int MarketingPayback = 1;

        #endregion

        #region Inverstments

        public const int InvestmentPlayersQuota  = MaxPlayers;
        public const int InvestmentPlacesQuota  = MaxWorkersPerPlayer * MaxPlayers;
        public const int InvestmentPlacesPerPlayerQuota = MaxWorkersPerPlayer;
        public const int InvestmentPayback = 30;

        #endregion

        #region Human Resources
        
        public const int HumanResourcesPlayersQuota  = 1;
        public const int HumanResourcesPlacesQuota  = 1;
        public const int HumanResourcesPlacesPerPlayerQuota = 1;

        #endregion

        public const int CostPerWorker = 5;
        public const int MoneyPerEachAttendant = 20;
    }
}