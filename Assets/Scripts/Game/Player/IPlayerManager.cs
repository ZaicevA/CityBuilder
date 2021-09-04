using System;
using Game.Data;
using Game.Economics.Utility;

namespace Game.Player
{
    public interface IPlayerManager
    {
        void SetPlayer(PlayerData data);
        int AddNewBuilding(DateTime builtDate, DateTime produceCompleteDate, int level);
        void UpgradeBuilding(int id, int level, DateTime produceCompleteDate);
        void AddCurrency(CurrencyAmount amount);
    }
}

