using System.Diagnostics;

namespace ContentPOC.Unit.Model.News
{
    //TODO Revisit modelling, should this be generic units or very specific due to the parent type

    [DebuggerDisplay("Value = {Value}")]
    public class Headline : Unit
    {
        public Headline(string value) => Value = value;

        //TODO Unit types need namespace consideration
        public override string[] Domain => new[] { "news", "headlines" };

        public string Value { get; private set; }
    }
}
