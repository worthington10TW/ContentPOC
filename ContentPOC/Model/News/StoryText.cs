namespace ContentPOC.Unit.Model.News
{
    public class StoryText : Unit
    {
        public StoryText(string value) => Value = value;

        public override string UnitType => nameof(StoryText);

        public string Value { get; }

        // TODO: abstract this away
        public override string ToString() => Value.ToString();
    }
}
