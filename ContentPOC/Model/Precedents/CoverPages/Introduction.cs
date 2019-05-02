using System.Diagnostics;

namespace ContentPOC.Unit.Model.Precendents.CoverPages
{
    public class Introduction : StringValue
    {
        public Introduction(string value) : base(value) { }

        public override string[] Domain => new[] { "news", "cover-page", "introduction" };
    }
}
