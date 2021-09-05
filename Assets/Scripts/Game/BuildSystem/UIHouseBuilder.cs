using Foundation;
using Game.Houses;
using Game.UI;
using UnityEngine;

namespace Game.Builder
{
    public class UIHouseBuilder : AbstractService<IHouseBuilder>, IHouseBuilder
    {
        [SerializeField] private HousesMenu _menu;
        
        public void Build(House house)
        {
            _menu.AddHouse(house);
        }
    }
}

