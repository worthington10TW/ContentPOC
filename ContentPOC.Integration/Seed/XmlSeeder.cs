using ContentPOC.NewsIngestor;
using ContentPOC.Unit.Model.News;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC.Seed
{
    public class XmlSeeder
    {
        private readonly IManager<NewsItem> _manager;

        public XmlSeeder(IManager<NewsItem> manager) =>
            _manager = manager;

        public async Task SeedAsync(string path)
        {
            if (!Directory.Exists(path))
            {
                await Console.Error.WriteAsync($"***** Seed directory {path} does not exist *****");
                return;
            }

            var files = Directory.GetFiles(path, "*.xml");        
            await Task.WhenAll(files.Select(file => PostData(file)));
        }

        private async Task PostData(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var xdoc = new XmlDocument();
                    xdoc.Load(stream);
                    var result = await _manager.SaveAsync(xdoc);
                    Console.WriteLine($"***** INSERTED: {result.Meta.Href} *****");
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync($"***** COULD NOT INGEST: {path} *****");
                await Console.Error.WriteLineAsync(ex.ToString());
            }
        }
    }
}
