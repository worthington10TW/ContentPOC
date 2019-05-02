using System.Diagnostics;

namespace ContentPOC.Unit.Model.News
{
    public class StoryText : StringValue
    {
        public StoryText(string value) : base(value) { }

        public override string[] Domain => new[]{"news", "story-text"};
    }
}
