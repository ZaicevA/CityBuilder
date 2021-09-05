using Game.Houses;

namespace Game.Startup
{
    public class SetupBuildingsStep : IStartupStep
    {
        private readonly IHousesManager _housesManager;

        public SetupBuildingsStep(IHousesManager housesManager)
        {
            _housesManager = housesManager;
        }

        public void Execute()
        {
            _housesManager.SetupBuildings();
        }
    }
}
