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
        
        public House BuildHouse(int id, HouseData data, DateAndTime produceCompleteDate, int levelId, int storedValue)
        {
            var result = new House(id, data, produceCompleteDate, levelId, storedValue);
            _houseBuilder.Build(result);
            return result;
        }
    }
}

