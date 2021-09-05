using Game.Houses;
using UnityEngine;

namespace Game.UI
{
    public class HousesMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private Transform _layoutGroup;
        [SerializeField] private HouseUI _houseUI;

        public void AddHouse(House house)
        {
            var go = Instantiate(_template, _layoutGroup);
            _template.GetComponent<HouseTemplate>().Init(house, _houseUI);
        }
    }
}
