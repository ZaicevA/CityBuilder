using Game.Economics.Utility;

namespace Game.Economics
{
    public interface IEconomicsManager
    {
        bool CanAfford(CurrencyAmount price);
        bool Buy(CurrencyAmount price);
        bool Earn(CurrencyAmount amount);
    }
}

