using ContentPOC.Unit;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public class InMemoryStore : IRepository
    {
        private readonly ConcurrentDictionary<string, IUnit> _store =
            new ConcurrentDictionary<string, IUnit>();

        public void Reset() => _store.Clear();

        public IUnit Get(Id id)
        {
            _store.TryGetValue(id.Value, out var value);
            return value;
        }

        public Task<IUnit> SaveAsync(IUnit unit)
        {
            //TODO This sucks, should be done better.....
            var collection = unit as UnitCollection;
            if (collection != null)
                foreach (var u in unit as UnitCollection)
                    Save(u);
            
            return Task.FromResult(Save(unit));
        }

        private IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(
                unit.Meta.Id.Value,
                unit,
                (key, existingUnit) => existingUnit = unit);

    }
}
