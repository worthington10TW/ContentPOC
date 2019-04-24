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
        private IConverter<News> _converter;
        private IRepository _repository;

        public NewsIngestorController(IConverter<News> converter, IRepository repository)
        {
            _converter = converter;
            _repository = repository;
        }

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
                _repository.Save(result);
                return Created(result.Href, result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _repository.Get(new Id(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
