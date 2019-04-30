using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Model;
using ContentPOC.Model.News;
using ContentPOC.Unit;
using ContentPOC.Unit.Model;
using ContentPOC.Unit.Model.News;
using System.Threading.Tasks;

namespace ContentPOC.NewsIngestor
{
    public class NewsManager
    {
        private readonly IConverter<NewsItem> _converter;
        private readonly IRepository _repository;
        private readonly INotificationQueue _queue;

        public NewsManager(
            IConverter<NewsItem> converter,
            IRepository repository,
            INotificationQueue queue)
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
                    // TODO: Change this to drive out the domain.
                    _queue.Queue(new RawNewsContentIngested { Location = r.Result.Meta.Href });
                    return r.Result;
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public IUnit Get(Id id) => _repository.Get(id);
    }
}
