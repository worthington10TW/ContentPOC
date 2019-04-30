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

        public IUnit Get(string area, Id id)
        {
            _store.TryGetValue(GenerateId(area, id), out var value);
            return value;
        }

        public Task<IUnit> SaveAsync(IUnit unit)
        {
            // TODO: run in parallel
            unit.Children.ForEach(u => Save(u));

            // TODO: this really needs IUnit not to be a DTO.  (We need to map so we don't bleed down.)
            return Task.FromResult(Save(unit));
        }

        private static string GenerateId(string area, Id id) => $"{area}{id.Value}";

        private IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(
                GenerateId(unit.Namespace, unit.Meta.Id),
                unit,
                (key, existingUnit) => existingUnit = unit);

    }
}
