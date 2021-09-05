using System;
using Foundation;
using Game.Data;
using Game.Economics.Utility;

namespace Game.Houses
{
    public class House
    {
        public int ID { get; private set; }
        public int LevelId { get; private set; }
        public DateTime ProduceCompleteDate { get; private set; }
        public DateTime CollectDate { get; private set; }
        public BuildingType Type => _data.BuildingType;

        
        private HouseData _data;

        public House(int id, HouseData data, Timings timings, int levelId)
        {
            ID = id;
            ProduceCompleteDate = timings.ProduceCompleteDate;
            CollectDate = timings.CollectDate;
            _data = data;
        }
        
        //Next two methods should be available only from HouseManager
        //this could be achieved if we add two interfaces, one of them would be accessible from presentation
        //and other would be accessible from HouseManager
        //didn't have time for this ¯\_(ツ)_/¯

        public void Upgrade(DateTime produceCompleteDate)
        {
            ProduceCompleteDate = produceCompleteDate;
            LevelId++;
        }

        public void Collect(DateTime collectDate)
        {
            CollectDate = collectDate;
        }

        public int GetStoredValue(DateTime when)
        {
            var levelId = InProduction(when) ?LevelId - 1 : LevelId;
            if(levelId < 0)
            {
                return 0;
            }

            var level = _data.Levels[levelId];
            var result = level.IncomePerSecond * (int)(when - CollectDate).TotalSeconds;
            return result >= level.StorageCapacity ? level.StorageCapacity : result;
        }

        public CurrencyAmount GetUpgradePrice()
        {
            if (LevelId + 1 < _data.Levels.Length)
            {
                return _data.Levels[LevelId + 1].UpgradePrice;
            }
            
            DebugOnly.Message($"House {ID} fully upgraded");
            return new CurrencyAmount();

        }

        public bool InProduction(DateTime when)
        {
            return when < ProduceCompleteDate;
        }
    }
}

