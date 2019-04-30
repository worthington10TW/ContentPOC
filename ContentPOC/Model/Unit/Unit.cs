using System.Collections.Generic;
using ContentPOC.Converter;
using ContentPOC.Model;
using Newtonsoft.Json;

namespace ContentPOC.Unit.Model
{
    public abstract class Unit : IUnit
    {
        public Meta Meta => new Meta(this);

        public abstract string Namespace { get; }

        public List<IUnit> Children { get; } = new List<IUnit>();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class Meta
    {
        public Meta(IUnit unit)
        {
            Id = new Id(string.Format("{0:X}", ToString()?.GetStableHashCode()));
            Area = new Area(unit?.Namespace.Split('.'));
        }

        [JsonIgnore]
        public virtual Id Id { get; }

        [JsonIgnore]
        public virtual Area Area { get; }

        public string Href => $"{string.Join("/", Area?.Value)}/{Id.Value}";
    }

    public class Id
    {
        public Id(string id) => Value = id;
        public string Value { get; }
    }

    public class Area
    {
        public Area(params string[] area) => Value = area;

        public string[] Value { get; }
    }
}
