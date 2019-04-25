namespace ContentPOC.Unit
{

    public class Story : Unit
    {
        public Story(string value) => Value = value;

        public override string UnitType => nameof(Story);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}
