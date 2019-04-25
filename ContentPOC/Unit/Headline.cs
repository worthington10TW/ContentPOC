namespace ContentPOC.Unit
{
    public class Headline : Unit
    {
        public Headline(string value) => Value = value;
        
        public override string UnitType => nameof(Headline);

        public string Value { get; private set; }

        public override string ToString() => Value.ToString();
    }
}
