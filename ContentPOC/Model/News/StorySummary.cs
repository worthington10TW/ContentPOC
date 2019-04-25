namespace ContentPOC.Unit.Model.News
{
    public class StorySummary : Unit
    {
        public StorySummary(string value) => Value = value;

        public override string UnitType => nameof(StorySummary);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}
