using ContentPOC.Converter;
using ContentPOC.NewsIngestor;
using ContentPOC.Unit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ContentPOC
{
    [Route("api/news")]
    public class NewsIngestorController : Controller
    {
        private readonly NewsManager _manager;

        public NewsIngestorController(NewsManager manager) => _manager = manager;

        //TODO lock down to XML
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var reader = XmlReader.Create(Request.Body))
            {
                var request = 
                    (NewsRequestXml)new XmlSerializer(typeof(NewsRequestXml))
                    .Deserialize(reader);
               var result = await _manager.SaveAsync(request);
                return Created(result.Href, result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _manager.Get(new Id(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
