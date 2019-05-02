namespace ContentPOC.Model
{
    public interface IMark
    {
        int StartCharacter { get; }

        int EndCharacter { get; }

        string Value { get; }
    }

    public class Mark : IMark
    {
        public Mark(int startCharacter, int endCharacter, string value)
        {
            StartCharacter = startCharacter;
            EndCharacter = EndCharacter;
            Value = value;
        }
        public int StartCharacter { get; }

        public int EndCharacter { get; }

        public string Value { get; }
    }
}