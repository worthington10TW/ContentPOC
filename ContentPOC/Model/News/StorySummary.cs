using System.Diagnostics;

namespace ContentPOC.Unit.Model.News
{
    [DebuggerDisplay("Value = {Value}")]
    public class StorySummary : Unit
    {
        public StorySummary(string value) => Value = value;

        public override string[] Domain => new[] { "news", "story-summaries" };

        public string Value { get; }
    }
}
