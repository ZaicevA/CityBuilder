using Game.Data;
using Game.Houses;

namespace Game.Startup
{
    public class BuildingsDataLoadingStep : IStartupStep
    {
        private readonly IDataLoader _dataLoader;
        private readonly IHousesManager _housesManager;

        public BuildingsDataLoadingStep(IDataLoader dataLoader, IHousesManager housesManager)
        {
            _dataLoader = dataLoader;
            _housesManager = housesManager;
        }

        public void Execute()
        {
            _housesManager.SetHousesData(_dataLoader.LoadHousesData());
        }
    }
}
