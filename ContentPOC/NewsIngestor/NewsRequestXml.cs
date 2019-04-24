using System.Xml.Serialization;

namespace ContentPOC
{
    [XmlRoot(ElementName = "news", Namespace = "")]
    public class NewsRequestXml
    {
        [XmlElement(DataType = "string", ElementName = "headline")]
        public string Headline { get; set; }
        
        [XmlElement(DataType = "string", ElementName = "summary")]
        public string Summary { get; set; }
        
        [XmlElement(DataType = "string", ElementName = "story")]
        public string Story { get; set; }
    }
}
