using Foundation;
using Game.Data;
using UnityEditor;
using UnityEngine;

namespace Game.SaveSystem
{
    public class PrefsSaveManager : AbstractService<ISaveManager>, ISaveManager
    {
        public void SavePlayer(PlayerData data)
        {
            var jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("PlayerProfile", jsonString);
            DebugOnly.Message($"Player saved");
        }
    }    
}

