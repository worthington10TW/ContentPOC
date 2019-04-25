namespace ContentPOC.Unit.Model.News
{
    public class StoryText : Unit
    {
        public StoryText(string value) => Value = value;

        public override string Namespace => "news/story-text";

        public string Value { get; }

        // TODO: abstract this away
        public override string ToString() => Value.ToString();
    }
}
