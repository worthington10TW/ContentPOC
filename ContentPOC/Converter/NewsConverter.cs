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
        private readonly DynamicNamespaceManager _namespaceManager;
        public NewsConverter(DynamicNamespaceManager namespaceManager) =>
            _namespaceManager = namespaceManager;

        public async Task<IUnit> CreateAsync(XmlDocument xml) =>
            new NewsItem(GetChildren(_namespaceManager.BuildXmlNamespaceManager(xml), xml));

        public IUnit[] GetChildren(XmlNamespaceManager manager, XmlDocument xdoc)
        {
            var list = new List<IUnit>();
            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/kh:document-title", manager).Select(x => new Headline(x)).ToArray());
            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/kh:mini-summary", manager).Select(x => new StorySummary(x)).ToArray());
            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/tr:secmain/core:para", manager).Select(x => new StoryText(x)).ToArray());
            return list.ToArray();
        }
        private string[] GetArray(XmlDocument xdoc, string xPath, XmlNamespaceManager manager)
        {
            var list = xdoc.SelectNodes(xPath, manager);
            var values = new List<string>();
            foreach (XmlNode para in list)
                values.Add(para.InnerText);
            return values.ToArray();
        }
    }
}