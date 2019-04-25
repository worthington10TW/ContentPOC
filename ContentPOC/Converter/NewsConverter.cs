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
                Headline = new Headline(xml.Headline),
                Summary = new Summary(xml.Summary),
                Story = new Story(xml.Story)
            };
        }
    }
}