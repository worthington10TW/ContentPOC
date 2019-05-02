namespace ContentPOC.Model.Precedents
{
    public class PrecedentItem : Unit.Model.Unit
    {
        public PrecedentItem(params IUnit[] children) => Children.AddRange(children);

        public override string[] Domain => new[] { "precedent" };
    }
}
