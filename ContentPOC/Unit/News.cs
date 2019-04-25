namespace ContentPOC.Unit
{
    public class News : UnitCollection
    { 
        public override string UnitType => nameof(News);

        public Headline Headline { get; set; }

        public Summary Summary { get; set; }

        public Story Story { get; set; }
    }

    public class Headline : Unit
    {
        public Headline(string value) => Value = value;

        public override string UnitType => nameof(Headline);

        public string Value { get;  }

        public override string ToString() => Value.ToString();
    }

    public class Summary : Unit
    {
        public Summary(string value) => Value = value;

        public override string UnitType => nameof(Summary);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
    
    public class Story : Unit
    {
        public Story(string value) => Value = value;

        public override string UnitType => nameof(Story);

        public string Value { get; }

        public override string ToString() => Value.ToString();
    }
}
