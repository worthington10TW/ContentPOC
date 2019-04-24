using ContentPOC.Converter;
using ContentPOC.Unit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ContentPOC
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/news-ingestor")]
    public class NewsIngestorController : Controller
    {
        private IConverter<News> _converter;

        public NewsIngestorController(IConverter<News> converter) =>
            _converter = converter;

        //TODO lock down to XML
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var reader = XmlReader.Create(Request.Body))
            {
                var request = 
                    (NewsRequestXml)new XmlSerializer(typeof(NewsRequestXml))
                    .Deserialize(reader);
                var result = await _converter.CreateAsync(request);
                return Created(result.Href, result);
            }
        }
    }
}
