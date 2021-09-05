using Game.Builder;
using Game.Data;
using Zenject;

namespace Game.Houses
{
    public class HousesFactory
    {
        private readonly IHouseBuilder _houseBuilder;
        
        public HousesFactory(IHouseBuilder builder)
        {
            _houseBuilder = builder;
        }
        
        public House BuildHouse(int id, HouseData data, Timings timings, int levelId)
        {
            var result = new House(id, data, timings, levelId);
            _houseBuilder.Build(result);
            return result;
        }
    }
}

