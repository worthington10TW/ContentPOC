using ContentPOC.Converter;
using ContentPOC.Model;
using Newtonsoft.Json;

namespace ContentPOC.Unit.Model
{
    public abstract class Unit : IUnit
    {
        public Meta Meta => new Meta(this);

        public abstract string Namespace { get; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class Meta
    {
        public Meta(IUnit unit)
        {
            Id = new Id(string.Format("{0:X}", ToString()?.GetStableHashCode()));
            Href = $"{unit?.Namespace}/{Id?.Value}";
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
