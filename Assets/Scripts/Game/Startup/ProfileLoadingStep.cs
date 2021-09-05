using Game.Data;
using Game.Player;

namespace Game.Startup
{
    public class ProfileLoadingStep : IStartupStep
    {
        private readonly IDataLoader _dataLoader;
        private readonly IPlayerManager _playerManager;

        public ProfileLoadingStep(IDataLoader loader, IPlayerManager playerManager)
        {
            _dataLoader = loader;
            _playerManager = playerManager;
        }
        
        public void Execute()
        {
            _playerManager.SetPlayer(_dataLoader.LoadPlayerData());
        }
    }
}
