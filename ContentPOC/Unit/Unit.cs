using ContentPOC.Converter;
using Newtonsoft.Json;

namespace ContentPOC.Unit
{
    public abstract class Unit : IUnit
    {
        public Meta Meta => new Meta(this);

        public abstract string UnitType { get; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class Meta
    {
        public Meta(IUnit unit)
        {
            Id = new Id(string.Format("{0:X}", ToString()?.GetStableHashCode()));
            Href = $"{unit?.UnitType?.ToLower()}/{Id?.Value}";
        }

        [JsonIgnore]
        public virtual Id Id { get; }

        [JsonIgnore]
        public virtual string Href { get; }

    }

    public class Id
    {
        public Id(string id) => Value = id;
        public string Value { get; set; }
    }
}
