using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model;
using ContentPOC.Unit.Model.News;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC
{
    /// <summary>
    /// News ingestor controller.
    /// </summary>
    [Route(NEWS_AREA)]
    public class NewsIngestorController : Controller
    {
        private const string NEWS_AREA = "news";
        private readonly IManager<NewsItem> _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContentPOC.NewsIngestorController"/> class.
        /// </summary>
        /// <param name="manager">Manager.</param>
        public NewsIngestorController(IManager<NewsItem> manager) => _manager = manager;

        //TODO lock down to XML
        /// <summary>
        /// Post this instance.
        /// </summary>
        /// <returns>The post.</returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var xdoc = new XmlDocument();
            xdoc.Load(Request.Body);
            var result = await _manager.SaveAsync(xdoc);

            //TODO I'm too lazy to make a real view model, this will do for now
            dynamic value = result;
            return Created(
                new Uri(new Uri("http://" + Request.Host.Value),
                result.Meta.Href),
                result);
            //}
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>The all.</returns>
        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll(NEWS_AREA));

        /// <summary>
        /// Get the specified id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="id">Identifier.</param>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id) => Get(id, NEWS_AREA);

        /// <summary>
        /// Get the specified area and id.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="area">Area.</param>
        /// <param name="id">Identifier.</param>
        [HttpGet("{area}/{id}")]
        public IActionResult Get(string area, Guid id) => Get(id, NEWS_AREA, area);

        private IActionResult Get(Guid id, params string[] areas)
        {
            var result = _manager.Get(areas, id);
            if (result == null)
                return NotFound();

            dynamic value = result;
            return Ok(value);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id) => Put(id, NEWS_AREA);

        private IActionResult Put(Guid id, params string[] areas)
        {
            var result = _manager.Get(areas, id);
            if (result == null)
                return NotFound();

            return Ok();
        }
    }
}
