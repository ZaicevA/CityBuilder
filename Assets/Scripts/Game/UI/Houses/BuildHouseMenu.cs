using Game.Data;
using Game.Economics;
using Game.Houses;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    /// <summary>
    /// Only works with one type of buildings
    /// </summary>
    public class BuildHouseMenu : MonoBehaviour
    {
        [Inject] private IHousesManager _housesManager;
        [Inject] private IEconomicsManager _economicsManager;

        [SerializeField] private TextMeshProUGUI _textPrice;
        [SerializeField] private TextMeshProUGUI _buildingName;

        private HouseData _data;

        private void Awake()
        {
            _data = _housesManager.GetHousesData()[0];
        }

        private void OnEnable()
        {
            //Use localization here
            _buildingName.text = _data.HouseName;
            var price = _data.Levels[0].UpgradePrice;
            _textPrice.text = $"{price.Currency} {price.Value}";
        }
        
        public void Build()
        {
            var price = _data.Levels[0].UpgradePrice;
            if(_economicsManager.Buy(price))
            {
                _housesManager.BuildHouse(_data.BuildingType);
            }
        }
    }
}
