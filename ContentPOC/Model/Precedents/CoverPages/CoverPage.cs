using ContentPOC.Model;
using System.Diagnostics;

namespace ContentPOC.Unit.Model.Precendents.CoverPages
{
    [DebuggerDisplay("Value = {Value}")]
    public class CoverPageItem : Unit
    {
        public CoverPageItem(params IUnit[] children) => Children.AddRange(children);

        public override string[] Domain => new[] { "precedent.cover-page" };
    }
}
