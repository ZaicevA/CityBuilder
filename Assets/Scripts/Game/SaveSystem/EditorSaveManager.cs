using Foundation;
using Game.Data;
using UnityEditor;
using UnityEngine;

namespace Game.SaveSystem
{
    public class EditorSaveManager : AbstractService<ISaveManager>, ISaveManager
    {
        public void SavePlayer(PlayerData data)
        {
            var playerData = Resources.Load<PlayerData>("PlayerData");
            playerData = data;
            EditorUtility.SetDirty(playerData);
            DebugOnly.Message($"Player saved");
        }
    }    
}

