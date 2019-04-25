namespace ContentPOC.Unit
{
    // TODO: rename this as StoryText
    public class Story : Unit
    {
        public Story(string value) => Value = value;

        public override string UnitType => nameof(Story);

        public string Value { get; }

        // TODO: abstract this away
        public override string ToString() => Value.ToString();
    }
}
