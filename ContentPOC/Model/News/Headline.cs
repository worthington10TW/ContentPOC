namespace ContentPOC.Unit.Model.News
{
    //TODO Revisit modelling, should this be generic units or very specific due to the parent type
    public class Headline : StringValue
    {
        public Headline(string value): base(value)
        {

        }

        //TODO Unit types need namespace consideration
        public override string[] Domain => new[] { "news", "headlines" };
    }
}
