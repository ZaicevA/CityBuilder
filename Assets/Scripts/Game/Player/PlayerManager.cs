using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Game.Data;
using Game.Economics.Utility;
using Game.SaveSystem;
using Zenject;

namespace Game.Player
{
    public class PlayerManager : AbstractService<IPlayerManager>, IPlayerManager
    {
        public ObserverList<IOnCurrencyChange> OnCurrencyChange { get; } = new ObserverList<IOnCurrencyChange>();
        
        [Inject] private ISaveManager _saveManager;
        
        private PlayerData _data;
        
        public void SetPlayer(PlayerData data)
        {
            _data = data;
            
            if(_data.BuiltHouses != null)
            {
                return;
            }

            DebugOnly.Message($"Built house data is null");
            _data.BuiltHouses = new List<BuiltHouseData>();
        }

        public DateTime GetLastSeenTime()
        {
            if (_data.LastSeen.Year == 0)
            {
                return DateTime.Now;
            }

            return _data.LastSeen.Get();
        }

        private void OnDestroy()
        {
            _data.LastSeen.Set(DateTime.Now);
            _saveManager.SavePlayer(_data);
        }

        public int AddNewBuilding(DateAndTime produceCompleteDate, int level, HouseData data)
        {
            var id = _data.BuiltHouses.Count;
            var houseData = new BuiltHouseData()
            {
                Type = data.BuildingType,
                HouseLevel = level,
                StoredResource = 0,
                ProduceCompleteDate = produceCompleteDate,
                Id = id
            };
            _data.BuiltHouses.Add(houseData);
            
            return id;
        }

        public void UpgradeBuilding(int houseId, int level, DateTime produceCompleteDate)
        {
            DebugOnly.Check(_data.BuiltHouses.Count > houseId, $"House with id {houseId},not presented in built houses");
            var house = _data.BuiltHouses[houseId];
            house.HouseLevel = level;
            house.ProduceCompleteDate.Set(produceCompleteDate);
            _data.BuiltHouses[houseId] = house;
        }

        public void UpdateHouseStoredResource(Dictionary<int,int> resources)
        {
            foreach (var resource in resources)
            {
                var house = _data.BuiltHouses[resource.Key];
                house.StoredResource = resource.Value;
                _data.BuiltHouses[resource.Key] = house;
            }
        }

        public void AddCurrency(CurrencyAmount amount)
        {
            var currentWallet = _data.PlayerWallet.FirstOrDefault(x => x.Currency == amount.Currency);
            DebugOnly.Check(currentWallet != null, $"Player dont have wallet for {amount.Currency}");
            
            currentWallet.Value += amount.Value;
            for (int i = 0; i < _data.PlayerWallet.Length; i++)
            {
                if(_data.PlayerWallet[i].Currency == currentWallet.Currency)
                {
                    _data.PlayerWallet[i] = currentWallet;
                }
            }
            
            foreach (var observer in OnCurrencyChange.Enumerate())
            {
                observer.Do();
            }
        }

        public int GetCurrencyValue(Currency type)
        {
            var currentWallet = _data.PlayerWallet.FirstOrDefault(x => x.Currency == type);
            DebugOnly.Check(currentWallet != null, $"Player dont have wallet for {type}");
            return currentWallet.Value;
        }

        public List<BuiltHouseData> GetBuiltHousesData()
        {
            return _data.BuiltHouses;
        }
    }
}

