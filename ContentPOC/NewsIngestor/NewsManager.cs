using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Model;
using ContentPOC.Model.News;
using ContentPOC.Unit.Model.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace ContentPOC.NewsIngestor
{
    public interface IManager<TUnit> where TUnit : IUnit
    {
        Task<IUnit> SaveAsync(XmlDocument request);

        Task<IUnit> SaveAsync(IUnit unit);

        IUnit Get(string[] domain, Guid id);

        List<IUnit> GetAll(params string[] domain);
    }

    public class NewsManager : IManager<NewsItem>
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

        public async Task<IUnit> SaveAsync(XmlDocument request) =>
            await SaveAsync(_converter.Create(request));

        public async Task<IUnit> SaveAsync(IUnit unit)
        {
            return await _repository
                .SaveAsync(unit)
                .ContinueWith(r =>
                {
                    // TODO: Change this to drive out the domain.
                    _queue.Queue(new RawNewsContentIngested { Location = r.Result.Meta.Href });
                    r.Result.Children.ForEach(c => _queue.Queue(new RawNewsContentIngested { Location = c.Meta.Href }));
                    return r.Result;
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        //TODO this was rushed dev, the idea of the location/areas/namespace has leaked everywhere! should update
        public IUnit Get(string[] domain, Guid id) => _repository.Get(domain, id);

        public List<IUnit> GetAll(params string[] domain) => _repository.GetAll(domain);
    }
}
