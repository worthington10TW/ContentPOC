using System.Diagnostics;

namespace ContentPOC.Unit.Model.News
{
    [DebuggerDisplay("Value = {Value}")]
    public class StoryText : Unit
    {
        public StoryText(string value) => Value = value;

        public override string Namespace => "news/story-text";

        public string Value { get; }
    }
}
