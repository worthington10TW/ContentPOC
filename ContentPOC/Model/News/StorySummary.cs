namespace ContentPOC.Unit.Model.News
{
    public class StorySummary : StringValue
    {
        public StorySummary(string value) : base(value) { }

        public override string[] Domain => new[] { "news", "story-summaries" };
    }
}
