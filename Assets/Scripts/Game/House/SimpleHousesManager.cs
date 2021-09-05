using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Game.Data;
using Game.Economics;
using Game.Economics.Utility;
using Game.Player;
using Zenject;

namespace Game.House
{
    public class SimpleHousesManager : AbstractService<IHousesManager>, IHousesManager
    {
        [Inject] private IPlayerManager _playerManager;
        [Inject] private IEconomicsManager _economicsManager;
        
        private HouseData[] _housesData;
        private Dictionary<int, House> _builtHouses;
        private HousesFabric _fabric;

        private void Awake()
        {
            _builtHouses = new Dictionary<int, House>();
            _fabric = new HousesFabric();
        }

        public void SetHousesData(HouseData[] data)
        {
            DebugOnly.Check(data != null, $"Houses data is null");
            DebugOnly.Check(data.Length != 0, $"Houses data is empty");
            _housesData = data;
        }

        public void SetupBuildings()
        {
            var builtHouses = _playerManager.GetBuiltHousesData();
            foreach (var house in builtHouses)
            {
                ConstructHouse(house.Id, house.Data, house.Timings, house.HouseLevel - 1);
            }
        }

        public HouseData[] GetHousesData()
        {
            return _housesData;
        }

        public void CollectHouseIncome(int houseId)
        {
            var house = _builtHouses[houseId];
            var data = GetHouseData(house.Type);
            var collectDate = DateTime.Now;
            var totalAmount = new CurrencyAmount()
            {
                Currency = data.ProducedCurrency,
                Value = house.GetStoredValue(collectDate)
            };
            
            house.Collect(collectDate);
            _economicsManager.Earn(totalAmount);
            _playerManager.CollectHouseIncome(houseId, collectDate);
        }

        public void BuildHouse(BuildingType type, int levelId = 0)
        {
            var data = GetHouseData(type);
            var produceTime = DateTime.Now + data.Levels[levelId].ProduceTime;
            var timings = new Timings()
            {
                BuiltDate = DateTime.Now,
                //Finish of building constructing is the moment when we start accumulating resource
                CollectDate = produceTime,
                ProduceCompleteDate = produceTime,
            };
            var id = _playerManager.AddNewBuilding(timings, levelId + 1, data);
            ConstructHouse(id,data,timings,levelId);
        }

        public bool IsUpgradeAvailable(int houseId)
        {
            var house = _builtHouses[houseId];
            var data = GetHouseData(house.Type);

            if (house.ProduceCompleteDate > DateTime.Now)
            {
                DebugOnly.Message($"House {houseId} in production");
                return false;
            }

            if (data.Levels.Length > house.LevelId + 1)
            {
                return _economicsManager.CanAfford(data.Levels[house.LevelId + 1].UpgradePrice);
            }
            
            DebugOnly.Message($"House {houseId} on maximum level");
            return false;
        }

        public void UpgradeHouse(int houseId)
        {
            var house = _builtHouses[houseId];
            var data = GetHouseData(house.Type);
            var newLevelId = house.LevelId + 1;
            var newLevel = data.Levels[newLevelId];
            var produceCompleteDate = DateTime.Now + newLevel.ProduceTime;
            _playerManager.UpgradeBuilding(houseId, newLevelId + 1, produceCompleteDate);
            house.Upgrade(produceCompleteDate);
        }

        private void ConstructHouse(int id, HouseData data, Timings timings, int levelId)
        {
            _builtHouses.Add(id, _fabric.BuildHouse(id, data, timings, levelId));
        }

        private HouseData GetHouseData(BuildingType type)
        {
            var data = _housesData.FirstOrDefault(x => x.BuildingType == type);
            DebugOnly.Check(data != null, $"No data of type {type}");
            return data;
        }
    }    
}

