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
        public BuildingType Type => _data.BuildingType;
        public int StoredResource { get; private set; }

        
        private HouseData _data;

        public House(int id, HouseData data, DateAndTime produceCompleteDate, int levelId, int storedResource)
        {
            ID = id;
            ProduceCompleteDate = produceCompleteDate.Get();
            LevelId = levelId;
            StoredResource = storedResource;
            _data = data;
        }
        
        //Next four methods should be available only from HouseManager
        //this could be achieved if we add two interfaces, one of them would be accessible from presentation
        //and other would be accessible from HouseManager
        //didn't have time for this ¯\_(ツ)_/¯

        public void TickForAbsentTime(DateTime lastSeen)
        {
            var result = 0;
            var now = DateTime.Now;
            //If building was constructing while player left
            if (lastSeen < ProduceCompleteDate)
            {
                var levelId = LevelId - 1;
                if (levelId >= 0)
                {
                    //if construction don't complete even now all income is calculated by old level 
                    var span = ProduceCompleteDate > now ? now - lastSeen : ProduceCompleteDate - lastSeen;
                    result += _data.Levels[levelId].IncomePerSecond * (int)span.TotalSeconds;
                }

                //if construction completes while player wasn't in the game
                if (now > ProduceCompleteDate)
                {
                    var span = now - ProduceCompleteDate;
                    result += _data.Levels[LevelId].IncomePerSecond * (int) span.TotalSeconds;
                }
            }
            else
            {
                var span = now - lastSeen;
                result += _data.Levels[LevelId].IncomePerSecond * (int) span.TotalSeconds;
            }

            StoredResource = StoredResource + result > _data.Levels[LevelId].StorageCapacity
                ? _data.Levels[LevelId].StorageCapacity
                : StoredResource + result;
        }

        public void Tick()
        {
            var levelId = InProduction(DateTime.Now) ?LevelId - 1 : LevelId;
            if(levelId < 0)
            {
                StoredResource = 0;
                return;
            }
            
            var level = _data.Levels[levelId];
            StoredResource += level.IncomePerSecond;
            StoredResource = StoredResource >= level.StorageCapacity ? level.StorageCapacity : StoredResource;
        }

        public void Upgrade(DateTime produceCompleteDate)
        {
            ProduceCompleteDate = produceCompleteDate;
            LevelId++;
        }

        public void Collect(DateTime collectDate)
        {
            StoredResource = 0;
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

