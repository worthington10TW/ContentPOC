using ContentPOC.Converter;
using ContentPOC.DAL;
using ContentPOC.HostedService;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Xunit;

namespace ContentPOC.Integration
{
    public class SetupTests : IDisposable
    {
        private readonly IWebHostBuilder _builder = Program.WebHostBuilder();
        private readonly TestServer _server;
        private readonly IServiceProvider _provider;

        public SetupTests()
        {
            _server = new TestServer(_builder);
            _provider = _server.Host.Services;
        }

        public void Dispose() => _server?.Dispose();

        [Fact]
        public void ShouldCreateNewsConverter_WhenCallingIConverter()
        {
            var converter = _provider.GetService<IConverter<NewsItem>>();

            converter.Should().NotBeNull();
            converter.Should().BeOfType<NewsConverter>();
        }

        [Fact]
        public void ShouldCreateInMemoryStore_WhenCallingIRepository()
        {
            var repository = _provider.GetService<IRepository>();

            repository.Should().NotBeNull();
            repository.Should().BeOfType<InMemoryStore>();
        }

        [Fact]
        public void ShouldSetupGlobalIUnitNotificationQueue()
        {
            var repository = _provider.GetService<IUnitNotificationQueue>();
            repository.Should().NotBeNull();
            repository.Should().BeOfType<RawNewsIngestedContentQueue>();

            var sameInstance = _provider.GetService<IUnitNotificationQueue>();

            sameInstance.Should().Be(repository);
        }

        [Fact]
        public void ShouldSetupNotificationHub()
        {
            _provider.GetServices<INotificationHub>().Should().NotBeNull();
            var repository = _provider.GetServices<IHostedService>();

            repository.Cast<NotificationHubService>()
                .Should().NotBeEmpty();
        }
    }
}
