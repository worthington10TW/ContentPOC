using System;
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
        private readonly ConcurrentDictionary<Id, IUnit> _store = 
            new ConcurrentDictionary<Id, IUnit>();

        public void Reset() => _store.Clear();

        public IUnit Get(Id id)
        {
            _store.TryGetValue(id, out var value);
            return value;
        }

        public IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(unit.Id, unit, (key, existingUnit) => existingUnit = unit);


    }
}
