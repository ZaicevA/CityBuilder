using Foundation;
using Game.Economics.Utility;
using Game.Player;
using Zenject;

namespace Game.Economics
{
    public class LocalEconomicsManager : AbstractService<IEconomicsManager>, IEconomicsManager
    {
        [Inject] private IPlayerManager _playerManager;

        public bool CanAfford(CurrencyAmount price)
        {
            return _playerManager.GetCurrencyValue(price.Currency) >= price.Value;
        }

        public bool Buy(CurrencyAmount price)
        {
            if (!CanAfford(price)) 
                return false;
            
            _playerManager.AddCurrency(new CurrencyAmount()
            {
                Currency = price.Currency,
                Value = - price.Value
            });
            return true;
        }

        public bool Earn(CurrencyAmount amount)
        {
            _playerManager.AddCurrency(amount);
            return true;
        }
    }    
}

