﻿using ContentPOC.Model;
using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model.News;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using static ContentPOC.Extensions.IUnitExtensions;

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
        public IActionResult GetAll() => Ok(
            _manager.GetAll(NEWS_AREA).Select(x => new {value = GetHeading(x), meta = x.Meta}));
            

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

        /// <summary>
        /// Put the specified id and data.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="data">Data.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            Guid id, 
            [FromBody]Dictionary<string, object> data) => 
            await PutUnitAsync(data, id, NEWS_AREA);

        private async Task<IActionResult> PutUnitAsync(
            Dictionary<string, object> data,
            Guid id, 
            params string[] areas)
            {
            if (data == null || !data.Any()) return BadRequest();

            return Ok(await _manager.SaveAsync(areas.ToUnit(id, data)));
            }

        private IActionResult Get(Guid id, params string[] areas)
        {
            var result = _manager.Get(areas, id);
            if (result == null)
                return NotFound();

            dynamic value = result;
            return Ok(value);
        }

        private static string GetHeading(IUnit unit)
        {
            var firstHeading = (Headline)unit.Children?.FirstOrDefault(x => x is Headline);
            return firstHeading?.Value ?? "No heading";
        }
    }
}
