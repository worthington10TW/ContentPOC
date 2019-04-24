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
                Headline = xml.Headline,
                Summary = xml.Summary,
                Story = xml.Story
            };
        }
    }
}