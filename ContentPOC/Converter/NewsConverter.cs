using ContentPOC.Unit;
using System.Threading.Tasks;

namespace ContentPOC.Converter
{
    public class NewsConverter : IConverter<Unit.News>
    {
        public async Task<IUnit> CreateAsync(NewsRequestXml xml)
        {
            return new News
            {
                new Headline(xml.Headline),
                new Summary(xml.Summary),
                new Story(xml.Story)
            };
        }
    }
}