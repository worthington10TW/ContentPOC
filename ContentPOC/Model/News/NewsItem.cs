using System;
using ContentPOC.Model;

namespace ContentPOC.Unit.Model.News
{
    public class NewsItem : Unit
    {
        public NewsItem(params IUnit[] children)
        {
            Children.AddRange(children);
        }

        public NewsItem(Guid[] childIds) => ChildIds.AddRange(childIds);

        public override string[] Domain => new[] { "news" };
    }
}
