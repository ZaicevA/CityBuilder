using System;
using System.Collections.Generic;
using Foundation;
using Game.Data;
using Game.Economics.Utility;

namespace Game.Player
{
    public interface IPlayerManager
    {
        public ObserverList<IOnCurrencyChange> OnCurrencyChange { get; }
        void SetPlayer(PlayerData data);
        int AddNewBuilding(Timings timings, int level, HouseData data);
        
        /// <summary>
        /// Update ONLY information about when player collects income from house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="collectDate"></param>
        void CollectHouseIncome(int houseId, DateTime collectDate);
        void UpgradeBuilding(int houseId, int level, DateTime produceCompleteDate);
        void AddCurrency(CurrencyAmount amount);
        int GetCurrencyValue(Currency type);
        List<BuiltHouseData> GetBuiltHousesData();
    }
}

