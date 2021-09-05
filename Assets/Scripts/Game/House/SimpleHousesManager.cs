using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Game.Builder;
using Game.Data;
using Game.Economics;
using Game.Economics.Utility;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Houses
{
    public class SimpleHousesManager : AbstractService<IHousesManager>, IHousesManager
    {
        [Inject] private IPlayerManager _playerManager;
        [Inject] private IEconomicsManager _economicsManager;
        [Inject] private IHouseBuilder _houseBuilder;
        
        private HouseData[] _housesData;
        private Dictionary<int, House> _builtHouses;
        private HousesFactory _factory;
        private bool _initialized;
        private readonly WaitForSeconds _tick = new WaitForSeconds(1f); 

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
                ConstructHouse(house.Id, GetHouseData(house.Type), house.ProduceCompleteDate, house.HouseLevel - 1,
                    house.StoredResource);
            }

            if (_builtHouses == null)
            {
                return;
            }
            
            var lastSeen = _playerManager.GetLastSeenTime();
            
            foreach (var house in _builtHouses)
            {
                house.Value.TickForAbsentTime(lastSeen);    
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
                Value = house.StoredResource
            };
            
            house.Collect(collectDate);
            _economicsManager.Earn(totalAmount);
        }

        public void BuildHouse(BuildingType type, int levelId = 0)
        {
            var data = GetHouseData(type);
            var produceTime = DateTime.Now + data.Levels[levelId].ProduceTime;
            var date = new DateAndTime(produceTime);
            var id = _playerManager.AddNewBuilding(date, levelId + 1, data);
            ConstructHouse(id,data,date,levelId , 0);
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
                return true;
            }
            
            DebugOnly.Message($"House {houseId} on maximum level");
            return false;
        }

        public void UpgradeHouse(int houseId)
        {
            if(!IsUpgradeAvailable(houseId))
            {
                return;
            }
            
            var house = _builtHouses[houseId];
            var data = GetHouseData(house.Type);
            var newLevelId = house.LevelId + 1;
            var newLevel = data.Levels[newLevelId];
            var produceCompleteDate = DateTime.Now + newLevel.ProduceTime;
            _playerManager.UpgradeBuilding(houseId, newLevelId + 1, produceCompleteDate);
            house.Upgrade(produceCompleteDate);
        }
        
        private void Awake()
        {
            if (!_initialized)
            {
                Init();
            }
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
            var storedResourceDict = _builtHouses.ToDictionary(house => house.Key, 
                house => house.Value.StoredResource);
            _playerManager.UpdateHouseStoredResource(storedResourceDict);
        }

        private void Init()
        {
            _builtHouses = new Dictionary<int, House>();
            _factory = new HousesFactory(_houseBuilder);
            StartCoroutine(HouseTicks());
            _initialized = true;
        }

        private IEnumerator HouseTicks()
        {
            while (true)
            {
                yield return _tick;
                foreach (var house in _builtHouses)
                {
                    house.Value.Tick();
                }
            }
        }

        private void ConstructHouse(int id, HouseData data, DateAndTime produceCompleteDate, int levelId, int storedIncome)
        {
            if (!_initialized)
            {
                Init();
            }
            
            _builtHouses.Add(id, _factory.BuildHouse(id, data, produceCompleteDate, levelId, storedIncome));
        }

        private HouseData GetHouseData(BuildingType type)
        {
            var data = _housesData.FirstOrDefault(x => x.BuildingType == type);
            DebugOnly.Check(data != null, $"No data of type {type}");
            return data;
        }
    }    
}

