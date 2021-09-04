using System;

//This should be described on server side
namespace Game.Economics.Utility
{
    [Serializable]
    public class CurrencyAmount
    {
        public Currency Currency;
        public int Value;
    }

    public enum Currency
    {
        Money = 0,
    }
}
