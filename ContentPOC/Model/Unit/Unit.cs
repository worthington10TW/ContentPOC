using ContentPOC.Converter;
using ContentPOC.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContentPOC.Unit.Model
{
    public abstract class Unit : IUnit
    {
        public IMeta Meta => new Meta(this);

        [JsonIgnore]
        public abstract string[] Domain { get; }

        public List<IUnit> Children { get; } = new List<IUnit>();

        public List<IMark> Marks { get; } = new List<IMark>();

        public bool ShouldSerializeChildren() => Children.Any();
    }

    public class Meta : IMeta
    {
        public Meta(IUnit unit)
        {
            Id = new Id(string.Format("{0:X}", JsonConvert.SerializeObject(
                unit,
                new JsonSerializerSettings
                {
                    ContractResolver = new IgnoreMetaSerializeContractResolver()
                }).GetStableHashCode()));
            Area = new Area(unit?.Domain);
        }

        [JsonIgnore]
        public virtual Id Id { get; }

        [JsonIgnore]
        public virtual Area Area { get; }

        public string Href => $"{string.Join("/", Area?.Value)}/{Id.Value}";
    }

    public interface IMeta
    {
        Id Id { get; }
        Area Area { get; }
        string Href { get; }
    }

    public class Id
    {
        public Id(string id) => Value = id;
        public string Value { get; }
    }

    public class Area
    {
        public Area(string[] area) => Value = area;

        public string[] Value { get; }
    }

    public class IgnoreMetaSerializeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Unit) && property.PropertyName == "Meta")
                property.ShouldSerialize = instance => false;

            return property;
        }
    }
}
