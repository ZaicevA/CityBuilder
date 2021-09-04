using System;
using Game.Economics.Utility;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "HouseData", menuName = "Data/HouseData")]
    public class HouseData : ScriptableObject
    {
        public HouseLevel[] Levels;
        public string HouseName;
        public Currency ProducedCurrency;
    }

    [Serializable]
    public struct HouseLevel
    {
        public CurrencyAmount UpgradePrice;
        public int StorageCapacity;
        public int IncomePerSecond;
        public DateTime ProduceTime => new DateTime(0,0,ProduceTimeDays,ProduceTimeHours,
            ProduceTimeMinutes,ProduceTimeSeconds);

        [SerializeField]
        private int ProduceTimeSeconds;
        [SerializeField]
        private int ProduceTimeMinutes;
        [SerializeField]
        private int ProduceTimeHours;
        [SerializeField]
        private int ProduceTimeDays;
    }
}

