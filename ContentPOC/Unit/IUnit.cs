namespace ContentPOC
{
    public interface IUnit
    {
        string Href { get; }

        Id Id { get; }
    }

    public class Id
    {
        public Id(string id) => Value = id;
        public string Value { get; set; }
    }
}