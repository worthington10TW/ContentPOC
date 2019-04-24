namespace ContentPOC.Unit
{
    public class News : UnitCollection
    { 
        public override string UnitType => nameof(News);

        public string Headline { get; set; }

        public string Summary { get; set; }

        public string Story { get; set; }
    }
}
