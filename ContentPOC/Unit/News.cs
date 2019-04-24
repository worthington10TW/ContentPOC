using System.Collections.Generic;
using ContentPOC.Converter;
using Newtonsoft.Json;

namespace ContentPOC.Unit
{
    public class News : IUnits
    {
        [JsonIgnore]
        public string Href => $"{nameof(News).ToLower()}/{string.Format("{0:X}", ToString().GetStableHashCode())}";

        public string Headline { get; set; }

        public string Summary { get; set; }

        public string Story { get; set; }

        [JsonIgnore]
        public List<IUnit> Units { get; } = new List<IUnit>();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
