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

        public Task<IUnit> SaveAsync(IUnit unit) =>
            Task.FromResult(_store.AddOrUpdate(unit.Id.Value, unit, (key, existingUnit) => existingUnit = unit));
    }
}
