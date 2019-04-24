using ContentPOC.Converter;
using ContentPOC.NewsIngestor;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            var converter = _provider.GetService<IConverter<Unit.News>>();

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

    }
}
