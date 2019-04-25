namespace ContentPOC.Unit.Model.News
{
    //TODO Revisit modelling, should this be generic units or very specific due to the parent type
    public class Headline : Unit
    {
        public Headline(string value) => Value = value;
        
        //TODO Unit types need namespace consideration
        public override string Namespace => "news/headline";
        
        public string Value { get; private set; }

        public override string ToString() => Value.ToString();
    }
}
