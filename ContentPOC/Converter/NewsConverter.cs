using ContentPOC.Model;
using ContentPOC.Unit.Model.News;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC.Converter
{
    public class NewsConverter : IConverter<NewsItem>
    {
        public async Task<IUnit> CreateAsync(XmlDocument xml) =>
            new NewsItem(GetChildren(xml));

        public IUnit[] GetChildren(XmlDocument xdoc)
        {
            var list = new List<IUnit>{new Headline(xdoc.SelectNodes("/document/body/secmain[1]/title")[0].InnerText)};
            list.AddRange(GetArray(xdoc, "/document/body/secmain[1]/para").Select(x => new StorySummary(x)).ToArray());
            list.AddRange(GetArray(xdoc, "/document/body/secmain[2]/para").Select(x => new StoryText(x)).ToArray());
            return list.ToArray();
        }
        private string[] GetArray(XmlDocument xdoc, string xPath)
        {
            var list = xdoc.SelectNodes(xPath);
            var values = new List<string>();
            foreach (XmlNode para in list)
                values.Add(para.InnerText);
            return values.ToArray();
        }
    }
}