using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ContentPOC.Converter
{
    public class DynamicNamespaceManager
    {
        public XmlNamespaceManager BuildXmlNamespaceManager(XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
            var firstChild = FindFirstChild(document);
            if (firstChild == null) return manager;

            GetNamespaceDictionary(firstChild)
                .ToList()
                .ForEach(x => manager.AddNamespace(x.Key, x.Value));
            return manager;
        }

        private static IDictionary<string, string> GetNamespaceDictionary(XmlNode node) =>
            !node.HasChildNodes ?
            node.CreateNavigator().GetNamespacesInScope(XmlNamespaceScope.All) :
            node.ChildNodes.Cast<XmlNode>()
                .Select(x => GetNamespaceDictionary(x))
                .SelectMany(d => d)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(g => g.Key, g => g.First());

        private static XmlNode FindFirstChild(XmlDocument document)
        {
            var node = document.FirstChild;
            while (node != null && node.NodeType != XmlNodeType.Element)
                node = node.NextSibling;
            return node;
        }
    }
}