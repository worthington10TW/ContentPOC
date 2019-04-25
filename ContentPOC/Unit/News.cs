namespace ContentPOC.Unit
{
    public class News : UnitCollection
    { 
        public override string UnitType => nameof(News);
    }

    public class Headline : Unit
    {
        public Headline(string value) => Value = value;

        public Headline() { }

        public override string UnitType => nameof(Headline);

        public string Value { get; private set; }

        public override string ToString() => Value.ToString();
    }

    public class Summary : Unit
    {
        public Summary(string value) => Value = value;

        public Summary() { }

        public override string UnitType => nameof(Summary);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
    
    public class Story : Unit
    {
        public Story(string value) => Value = value;

        public Story() { }

        public override string UnitType => nameof(Story);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}
