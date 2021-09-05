using Game.Houses;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HouseTemplate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText; 
        private House _house;
        private HouseUI _houseUI;

        public void Init(House house, HouseUI ui)
        {
            _house = house;
            _houseUI = ui;
            _nameText.text = $"House {house.ID}";
        }

        public void OnClick()
        {
            _houseUI.Setup(_house);
        }
    }
}
