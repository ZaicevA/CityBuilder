using Foundation;
using UnityEngine;

namespace Game.Data
{
    public class DummyDataLoader : AbstractService<IDataLoader>, IDataLoader
    {
        public HouseData[] LoadHousesData()
        {
            var result = new HouseData[1];
            result[0] = Resources.Load<HouseData>("HouseData");
            return result;
        }

        public PlayerData LoadPlayerData()
        {
            return Resources.Load<PlayerData>("PlayerData");
        }
    }
}

