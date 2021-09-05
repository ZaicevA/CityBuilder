using Foundation;
using Game.Economics.Utility;
using Game.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class WalletUI : AbstractBehaviour, IOnCurrencyChange
    {
        [Inject] private IPlayerManager _playerManager;

        [SerializeField] private Currency _type;
        [SerializeField] private TextMeshProUGUI _walletText;

        protected override void OnEnable()
        {
            base.OnEnable();
            Observe(_playerManager.OnCurrencyChange);
            _walletText.text = $"{_type} {_playerManager.GetCurrencyValue(_type)}";
        }

        public void Do()
        {
            _walletText.text = $"{_type} {_playerManager.GetCurrencyValue(_type)}";
        }
    }
}
