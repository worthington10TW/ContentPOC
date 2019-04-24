using ContentPOC.NewsIngestor;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ContentPOC.Integration
{
    public class InMemoryStoreTests : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly InMemoryStore _store;

        public InMemoryStoreTests()
        {
            _testServer = new TestServer(Program.WebHostBuilder());
            _store = (InMemoryStore)_testServer.Host.Services.GetService<IRepository>();
        }

        [Fact]
        public void WhenNoResult_ShouldReturnNull() =>
            _store.Get(new Id(Guid.NewGuid().ToString()))
                .Should()
                .BeNull();

        [Fact]
        public void WhenNewResult_ShouldInsert()
        {
            var id = new Id("amazingUnit");
            var unit = new TestUnit { Id = id, Href = "i-live/here" };
            _store.Save(unit);

            _store.Get(id).Should().BeEquivalentTo(unit);
        }

        [Fact]
        public void WhenClearning_ShouldBeEmpty()
        {
            var id = new Id("amazingUnit");
            var unit = new TestUnit { Id = id, Href = "i-live/here" };
            _store.Save(unit);
            _store.Get(id).Should().BeEquivalentTo(unit);
            _store.Reset();

            _store.Get(id).Should().BeNull();
        }

        [Fact]
        public void WhenIdMatches_ShouldReplace()
        {
            var id = new Id("amazingUnit");
            _store.Save(new TestUnit { Id = id, Href = "i-live/here" });
            var toReplace = new TestUnit { Id = id, Href = "i-dont-look/the-same" };
            _store.Save(toReplace);

            _store.Get(id).Should().BeEquivalentTo(toReplace);
        }
        
        public void Dispose()
        {
            _store.Reset();
            _testServer.Dispose();
        }

        public class TestUnit : IUnit
        {
            public string Href { get; set; }

            public Id Id { get; set; }
        }
    }
}
