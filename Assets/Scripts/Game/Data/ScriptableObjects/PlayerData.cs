using System;
using System.Collections.Generic;
using Game.Economics.Utility;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public CurrencyAmount[] PlayersWallet;
        public List<BuiltHouseData> BuiltHouses;
    }

    public struct BuiltHouseData
    {
        public DateTime BuiltDate;
        public DateTime CollectDate;
        public DateTime ProduceCompleteDate;
        public int HouseLevel;
    }
}