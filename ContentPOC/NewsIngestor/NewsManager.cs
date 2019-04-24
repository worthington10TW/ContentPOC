using ContentPOC.Converter;
using ContentPOC.Unit;
using System.Threading.Tasks;

namespace ContentPOC.NewsIngestor
{
    public class NewsManager
    {
        private readonly IConverter<News> _converter;
        private readonly IRepository _repository;

        public NewsManager(IConverter<News> converter, IRepository repository)
        {
            _converter = converter;
            _repository = repository;
        }

        public async Task<IUnit> SaveAsync(NewsRequestXml request)
        {

            var result = await _converter.CreateAsync(request);
            return _repository.Save(result);
        }

        public IUnit Get(Id id) => _repository.Get(id);
    }
}
