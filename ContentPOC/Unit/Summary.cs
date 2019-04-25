namespace ContentPOC.Unit
{
    public class Summary : Unit
    {
        public Summary(string value) => Value = value;

        public override string UnitType => nameof(Summary);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}
