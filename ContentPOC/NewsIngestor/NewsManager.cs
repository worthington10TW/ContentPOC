using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Unit;
using System.Threading.Tasks;

namespace ContentPOC.NewsIngestor
{
    public class NewsManager
    {
        private readonly IConverter<News> _converter;
        private readonly IRepository _repository;
        private readonly IUnitNotificationQueue _queue;

        public NewsManager(
            IConverter<News> converter,
            IRepository repository,
            IUnitNotificationQueue queue)
        {
            _converter = converter;
            _repository = repository;
            _queue = queue;
        }

        public async Task<IUnit> SaveAsync(NewsRequestXml request)
        {
            var result = await _converter.CreateAsync(request);
            return await _repository
                .SaveAsync(result)
                .ContinueWith(r =>
                {
                    _queue.Queue(r.Result);
                    return r.Result;
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public IUnit Get(Id id) => _repository.Get(id);
    }
}
