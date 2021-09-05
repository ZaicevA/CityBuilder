using System;
using System.Collections.Generic;
using Game.Data;
using Game.Houses;
using Game.Player;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Startup
{
    public class StartupController : MonoBehaviour
    {
        [Inject] private IPlayerManager _playerManager;
        [Inject] private IHousesManager _housesManager;
        [Inject] private IDataLoader _dataLoader;
        [Inject] private ILoadingController _loadingController;

        private void Awake()
        {
            _loadingController.SetLoading(true);
            foreach (var step in MainSteps())
            {
                step.Execute();
            }
            _loadingController.SetLoading(false);
        }

        private IEnumerable<IStartupStep> MainSteps()
        {
            yield return new ProfileLoadingStep(_dataLoader, _playerManager);
            yield return new BuildingsDataLoadingStep(_dataLoader, _housesManager);
            yield return new SetupBuildingsStep(_housesManager);
        }
    }
}
