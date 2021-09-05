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
        DateTime GetLastSeenTime();
        int AddNewBuilding(DateAndTime produceCompleteDate, int level, HouseData data);
        void UpgradeBuilding(int houseId, int level, DateTime produceCompleteDate);
        void UpdateHouseStoredResource(Dictionary<int,int> resources);
        void AddCurrency(CurrencyAmount amount);
        int GetCurrencyValue(Currency type);
        List<BuiltHouseData> GetBuiltHousesData();
    }
}

