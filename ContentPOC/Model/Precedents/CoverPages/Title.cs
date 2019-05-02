using System.Diagnostics;

namespace ContentPOC.Unit.Model.Precendents.CoverPages
{
    public class Title : StringValue
    {
        public Title(string value) : base(value) { }

        public override string[] Domain => new[]{"news", "cover-page", "title"};
    }
}
