using ContentPOC.NewsIngestor;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC.Seed
{
    public class XmlSeeder
    {
        private readonly NewsManager _manager;

        public XmlSeeder(NewsManager manager) =>
            _manager = manager;

        public async Task SeedAsync()
        {
            var files = Directory.GetFiles(Path.Combine("Seed", "Data"), "*.xml");
            await Task.WhenAll(files.Select(file => PostData(file)));
        }

        private async Task PostData(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var xdoc = new XmlDocument();
                xdoc.Load(stream);
                var result = await _manager.SaveAsync(xdoc);
                Console.WriteLine($"***** INSERTED: {result.Meta.Href} *****");
            }
        }
    }
}
