using Game.Data;

namespace Game.House
{
    public interface IHousesManager
    {
        void SetHousesData(HouseData[] data);
        void SetupBuildings();
        HouseData[] GetHousesData();
        void CollectHouseIncome(int houseId);
        void BuildHouse(BuildingType type, int levelId = 0);
        bool IsUpgradeAvailable(int houseId);
        void UpgradeHouse(int houseId);
    }
}

