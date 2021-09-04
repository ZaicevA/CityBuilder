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

        public int AddNewBuilding(DateTime builtDate, DateTime produceCompleteDate, int level)
        {
            var id = _data.BuiltHouses.Count;
            var houseData = new BuiltHouseData()
            {
                BuiltDate = builtDate,
                CollectDate = builtDate,
                HouseLevel = level,
                ProduceCompleteDate = produceCompleteDate,
                Id = id
            };
            _data.BuiltHouses.Add(houseData);
            
            return id;
        }

        public void UpgradeBuilding(int id, int level, DateTime produceCompleteDate)
        {
            DebugOnly.Check(_data.BuiltHouses.Count > id, $"House with id {id},not presented in built houses");
            var house = _data.BuiltHouses[id];
            house.HouseLevel = level;
            house.ProduceCompleteDate = produceCompleteDate;
            _data.BuiltHouses[id] = house;
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
            
            _saveManager.SavePlayer(_data);
        }

        public int GetCurrencyValue(Currency type)
        {
            var currentWallet = _data.PlayerWallet.FirstOrDefault(x => x.Currency == type);
            DebugOnly.Check(currentWallet != null, $"Player dont have wallet for {type}");
            return currentWallet.Value;
        }
    }
}

