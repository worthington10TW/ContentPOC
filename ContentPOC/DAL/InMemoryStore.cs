using ContentPOC.Model;
using ContentPOC.Unit.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public class InMemoryStore : IRepository
    {
        private readonly ConcurrentDictionary<string, IUnit> _store =
            new ConcurrentDictionary<string, IUnit>();

        public void Reset() => _store.Clear();

        public IUnit Get(string[] domain, Guid id)
        {
            _store.TryGetValue(GenerateId(domain, id), out var value);
            return value;
        }

        public Task<IUnit> SaveAsync(IUnit unit)
        {
            unit.Children.AsParallel().ForAll(u => Save(u));
            // TODO: this really needs IUnit not to be a DTO.  (We need to map so we don't bleed down.)
            return Task.FromResult(Save(unit));
        }

        private static string GenerateId(string[] domain, Guid id) => $"{string.Join(".", domain)}.{id}";

        private IUnit Save(IUnit unit) =>
            _store.AddOrUpdate(
                GenerateId(unit.Domain, unit.Meta.Id),
                unit,
                (key, existingUnit) => existingUnit = unit);

        public List<IUnit> GetAll(string[] area) =>
            _store.Where(x => x.Value?.Domain.All(d => area.Contains(d)) ?? false)
            .Select(x => x.Value)
            .ToList();
    }
}
