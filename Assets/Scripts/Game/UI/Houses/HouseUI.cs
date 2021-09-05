using System;
using System.Collections;
using Game.Economics;
using Game.Houses;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class HouseUI : MonoBehaviour
    {
        [Inject] private IHousesManager _housesManager;
        [Inject] private IEconomicsManager _economicsManager;

        [SerializeField] private TextMeshProUGUI _incomeText;
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _produceTimer;
        
        private House _house;
        private readonly WaitForSeconds _delay = new WaitForSeconds(1f);

        public void Setup(House house)
        {
            StopAllCoroutines();
            _house = house;
            StartCoroutine(UpdateRoutine());
        }

        public void Upgrade()
        {
            _housesManager.UpgradeHouse(_house.ID);
        }

        public void Collect()
        {
            if(_economicsManager.Buy(_house.GetUpgradePrice()))
            {
                _housesManager.CollectHouseIncome(_house.ID);
            }
        }

        private IEnumerator UpdateRoutine()
        {
            while(true)
            {
                _upgradeCostText.gameObject.SetActive(_housesManager.IsUpgradeAvailable(_house.ID));
                var price = _house.GetUpgradePrice();
                _upgradeCostText.text = $"{price.Currency} {price.Value}";
                _incomeText.text = _house.GetStoredValue(DateTime.Now).ToString();
                _produceTimer.gameObject.SetActive(_house.InProduction(DateTime.Now));
                _produceTimer.text = (_house.ProduceCompleteDate - DateTime.Now).ToString();
                yield return _delay;
            }
        }
    }
}
