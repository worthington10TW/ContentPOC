using ContentPOC.Converter;
using Newtonsoft.Json;

namespace ContentPOC.Unit
{
    public class News : IUnit
    {
        [JsonIgnore]
        public string Href => $"{nameof(News).ToLower()}/{string.Format("{0:X}", ToString().GetStableHashCode())}";

        public string Headline { get; set; }

        public string Summary { get; set; }

        public string Story { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
