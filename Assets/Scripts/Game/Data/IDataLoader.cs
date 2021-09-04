namespace Game.Data
{
    public interface IDataLoader
    {
        HouseData[] LoadHousesData();
        PlayerData LoadPlayerData();
    }
}

