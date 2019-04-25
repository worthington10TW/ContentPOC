using ContentPOC.Model;
using ContentPOC.Unit.Model;
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
            // TODO: run in parallel
            var collection = unit as UnitCollection;
            if (collection != null)
                collection.ForEach(u => Save(u));
            
            // TODO: this really needs IUnit not to be a DTO.  (We need to map so we don't bleed down.)
            return Task.FromResult(Save(unit)); 
        }

        private IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(
                unit.Meta.Id.Value,
                unit,
                (key, existingUnit) => existingUnit = unit);

    }
}
