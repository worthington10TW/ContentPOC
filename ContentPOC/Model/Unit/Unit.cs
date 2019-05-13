using ContentPOC.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContentPOC.Unit.Model
{
    public abstract class Unit : IUnit
    {
        protected Unit() => Meta = new Meta(this);

        public IMeta Meta { get; }

        [JsonIgnore]
        public abstract string[] Domain { get; }

        public List<IUnit> Children { get; } = new List<IUnit>();

        public List<Guid> ChildIds { get; } = new List<Guid>();

        public List<IMark> Marks { get; } = new List<IMark>();

        public bool ShouldSerializeChildren() => Children.Any();
    }

    public class Meta : IMeta
    {
        public Meta(IUnit unit)
        {
            Area = new Area(unit?.Domain);
            Type = unit.Domain;
        }

        [JsonIgnore]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public void SetId(Guid id) => Id = id;

        [JsonIgnore]
        public virtual Area Area { get; }

        public string Href => $"{string.Join("/", Area?.Value)}/{Id}";

        public string[] Type { get; }
    }

    public interface IMeta
    {
        Guid Id { get; }
        Area Area { get; }
        string Href { get; }
        void SetId(Guid id);
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
