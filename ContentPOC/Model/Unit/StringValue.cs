using System.Diagnostics;

namespace ContentPOC.Unit.Model
{
    [DebuggerDisplay("Value = {Value}")]
    public abstract class StringValue : Unit
    {
        public StringValue(string value) => Value = value;

        public abstract override string[] Domain { get; }

        public string Value { get; }
    }
}
