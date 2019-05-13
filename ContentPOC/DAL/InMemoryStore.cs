using ContentPOC.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentPOC.DAL
{
    public class InMemoryStore : IRepository
    {
        private readonly ConcurrentDictionary<string, DTO> _store =
            new ConcurrentDictionary<string, DTO>();

        public void Reset() => _store.Clear();

        public IUnit Get(string[] domain, Guid id)
        {
            _store.TryGetValue(GenerateId(domain, id), out var value);
            value?.Unit?.Children?.AddRange(GetChildren(value?.ChildrenIds));
            return value?.Unit;
        }

        public Task<IUnit> SaveAsync(IUnit unit)
        {
            unit.Children.AsParallel().ForAll(u => Save(u));
            return Task.FromResult(Save(unit));
        }

        public List<IUnit> GetAll(string[] area)=>
            _store.Where(x => x.Value?.Unit.Domain.All(d => area.Contains(d)) ?? false)
              .Select(x => {
                  var value = x.Value;
                  value.Unit.Children.AddRange(GetChildren(value.ChildrenIds));
                  return value.Unit;}).ToList();

        private static string GenerateId(string[] domain, Guid id) =>
            $"{string.Join(".", domain)}.{id}";

        private IUnit Save(IUnit unit)
        {
            var dto = new DTO(unit);
            return _store.AddOrUpdate(
                 GenerateId(unit.Domain, unit.Meta.Id), dto,
                 (key, existingUnit) => existingUnit = dto).Unit;
        }

        private IEnumerable<IUnit> GetChildren(DTO.Child[] children) => children?.Select(x => Get(x.Domain, x.Id));         
    }

    public class DTO
    {
        public DTO(IUnit unit)
        {
            Unit = unit;
            //ChildrenIds = unit?.Children?.Select(x => new Child(x.Domain, x.Meta.Id))
                //.ToArray();
        }

        public IUnit Unit { get; private set; }
        public Child[] ChildrenIds { get; private set; } = new List<Child>().ToArray();

        public class Child
        {
            public Child(string[] domain, Guid id)
            {
                Domain = domain;
                Id = id;
            }
            public string[] Domain { get; set; }
            public Guid Id { get; set; }
        }
    }

}
