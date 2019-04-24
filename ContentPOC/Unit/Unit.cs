using ContentPOC.Converter;
using Newtonsoft.Json;

namespace ContentPOC.Unit
{
    public abstract class Unit : IUnit
    {
        [JsonIgnore]
        public Id Id => new Id(string.Format("{0:X}", ToString().GetStableHashCode()));

        [JsonIgnore]
        public string Href => $"{UnitType.ToLower()}/{Id.Value}";

        public abstract string UnitType { get; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
