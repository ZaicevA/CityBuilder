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
                DebugOnly.Message($"Built house data is null");
                return;
            }

            _data.BuiltHouses = new List<BuiltHouseData>();
        }

        public int AddNewBuilding(Timings timings, int level, HouseData data)
        {
            var id = _data.BuiltHouses.Count;
            var houseData = new BuiltHouseData()
            {
                Data = data,
                HouseLevel = level,
                Timings = timings,
                Id = id
            };
            _data.BuiltHouses.Add(houseData);
            
            return id;
        }

        public void CollectHouseIncome(int houseId, DateTime collectDate)
        {
            DebugOnly.Check(_data.BuiltHouses.Count > houseId, $"House with id {houseId},not presented in built houses");
            var house = _data.BuiltHouses[houseId];
            house.Timings.CollectDate = collectDate;
            _data.BuiltHouses[houseId] = house;
            _saveManager.SavePlayer(_data);
        }

        public void UpgradeBuilding(int houseId, int level, DateTime produceCompleteDate)
        {
            DebugOnly.Check(_data.BuiltHouses.Count > houseId, $"House with id {houseId},not presented in built houses");
            var house = _data.BuiltHouses[houseId];
            house.HouseLevel = level;
            house.Timings.ProduceCompleteDate = produceCompleteDate;
            _data.BuiltHouses[houseId] = house;
            _saveManager.SavePlayer(_data);
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
            
            _saveManager.SavePlayer(_data);
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

