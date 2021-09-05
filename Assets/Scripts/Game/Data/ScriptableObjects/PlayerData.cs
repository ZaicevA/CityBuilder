using System;
using System.Collections.Generic;
using Game.Economics.Utility;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    [Serializable]
    public class PlayerData : ScriptableObject
    {
        public CurrencyAmount[] PlayerWallet;
        public List<BuiltHouseData> BuiltHouses;
        public DateAndTime LastSeen;
    }

    [Serializable]
    public struct BuiltHouseData
    {
        public BuildingType Type;
        public DateAndTime ProduceCompleteDate;
        public int HouseLevel;
        public int Id;
        public int StoredResource;
    }

    [Serializable]
    public struct DateAndTime
    {
        public int Second;
        public int Minute;
        public int Hour;
        public int Day;
        public int Month;
        public int Year;

        public DateAndTime(DateTime dateTime)
        {
            Second = dateTime.Second;
            Minute = dateTime.Minute;
            Hour = dateTime.Hour;
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
        }

        public void Set(DateTime dateTime)
        {
            Second = dateTime.Second;
            Minute = dateTime.Minute;
            Hour = dateTime.Hour;
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
        }

        public DateTime Get()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second);
        }
    }
}