﻿using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ContentPOC
{
    [Route("api/" + NEWS_AREA)]
    public class NewsIngestorController : Controller
    {
        private const string NEWS_AREA = "news";
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

                //TODO I'm too lazy to make a real view model, this will do for now
                dynamic value = result;
                return Created(
                    new Uri(new Uri("http://" + Request.Host.Value),
                    result.Meta.Href),
                    result);
            }
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll(NEWS_AREA));

        [HttpGet("{id}")]
        public IActionResult Get(string id) => Get(new Id(id), NEWS_AREA);

        [HttpGet("{area}/{id}")]
        public IActionResult Get(string area, string id) => Get(new Id(id), NEWS_AREA, area);

        private IActionResult Get(Id id, params string[] areas)
        {
            var result = _manager.Get(areas, id);
            if (result == null)
                return NotFound();

            dynamic value = result;
            return Ok(value);
        }

    }
}
