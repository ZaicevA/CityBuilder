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
            //JSON deserialization don't work dunno why :(
            PlayerPrefs.DeleteAll();
            DebugOnly.Message(PlayerPrefs.GetString("PlayerProfile"));
            var data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerProfile"));
            if (PlayerPrefs.GetString("PlayerProfile") == "")
            {
                data =Resources.Load<PlayerData>("PlayerData");
            }
            
            return data;
        }
    }
}

