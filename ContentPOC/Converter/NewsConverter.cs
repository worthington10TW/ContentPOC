using ContentPOC.Model;
using ContentPOC.Unit.Model.News;
using System.Threading.Tasks;

namespace ContentPOC.Converter
{
    public class NewsConverter : IConverter<NewsItem>
    {
        public async Task<IUnit> CreateAsync(NewsRequestXml xml)
        {
            return new NewsItem
            (
                new Headline(xml.Headline),
                new StorySummary(xml.Summary),
                new StoryText(xml.Story)
            );
        }
    }
}