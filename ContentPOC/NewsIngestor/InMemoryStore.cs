using System.Collections.Concurrent;

namespace ContentPOC.NewsIngestor
{
    public interface IRepository
    {
        IUnit Save(IUnit unit);

        IUnit Get(Id id);
    }

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

        public IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(unit.Id.Value, unit, (key, existingUnit) => existingUnit = unit);


    }
}
