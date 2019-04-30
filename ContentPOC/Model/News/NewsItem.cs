using ContentPOC.Model;

namespace ContentPOC.Unit.Model.News
{
    public class NewsItem : Unit
    { 
        public NewsItem(params IUnit[] children) => Children.AddRange(children);

        public override string Namespace => "news";
    }
}
