using ContentPOC.Model;
using ContentPOC.Unit.Model;
using ContentPOC.Unit.Model.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentPOC.Extensions;
using System.Xml;

namespace ContentPOC.Converter
{
    public class NewsConverter : IConverter<NewsItem>
    {
        private readonly DynamicNamespaceManager _namespaceManager;
        public NewsConverter(DynamicNamespaceManager namespaceManager) =>
            _namespaceManager = namespaceManager;

        public IUnit Create(XmlDocument xml)
        {
            if (xml == null) throw new ArgumentNullException(nameof(xml));

            var news = new NewsItem(GetChildren(_namespaceManager.BuildXmlNamespaceManager(xml), xml));
            news.Meta.SetId(news.ToGuid());
            return news;
        }

        //TODO The stream should be mapped while reading to maintain order type
        public IUnit[] GetChildren(XmlNamespaceManager manager, XmlDocument xdoc)
        {
            var list = new List<IUnit>();
            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/kh:document-title", manager)
                .Select(x => CreateUnit<Headline>(x)).ToArray());

            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/kh:mini-summary", manager)
                 .Select(x => CreateUnit<StorySummary>(x)).ToArray());

            list.AddRange(GetArray(xdoc, "/kh:document/kh:body/tr:secmain/core:para", manager)
                 .Select(x => CreateUnit<StoryText>(x)).ToArray());
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

        private TUnit CreateUnit<TUnit>(string value) where TUnit : StringValue, IUnit
        {
            var unit = (TUnit)Activator.CreateInstance(typeof(TUnit), args: value);
            var id = unit.ToGuid();
            unit.Meta.SetId(id);
            return unit;

        }
    }
}