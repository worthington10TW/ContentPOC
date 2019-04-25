using ContentPOC.DAL;
using ContentPOC.Unit;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
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
        public async Task WhenNewResult_ShouldInsert()
        {
            var id = new Id("amazingUnit");
            var unit = new TestUnit
            {
                Meta = new TestMeta(id, new TestUnit()),
                Value = "i-live/here"
            };
            await _store.SaveAsync(unit);

            _store.Get(id).Should().BeEquivalentTo(unit);
        }

        [Fact]
        public async Task WhenClearing_ShouldBeEmpty()
        {
            var id = new Id("amazingUnit");
            var unit = new TestUnit
            {
                Meta = new TestMeta(id, new TestUnit()),
                Value = "i-live/here"
            };
            await _store.SaveAsync(unit);
            _store.Get(id).Should().BeEquivalentTo(unit);
            _store.Reset();

            _store.Get(id).Should().BeNull();
        }

        [Fact]
        public async Task WhenIdMatches_ShouldReplace()
        {
            await _store.SaveAsync(new TestUnit
            {
                Meta = new TestMeta(new Id("amazingUnit"), new TestUnit()),
                Value = "i-live/here"
            });
            var toReplace = new TestUnit
            {
                Meta = new TestMeta(new Id("amazingUnit"), new TestUnit()),
                Value = "i-dont-look/the-same"
            };

            await _store.SaveAsync(toReplace);

            _store.Get(new Id("amazingUnit")).Should().BeEquivalentTo(toReplace);
        }

        public void Dispose()
        {
            _store.Reset();
            _testServer.Dispose();
        }

        public class TestUnit : IUnit
        {
            public string UnitType => nameof(TestUnit);

            public Meta Meta { get; set; }

            public string Value { get; set; }
        }

        public class TestMeta : Meta
        {
            public TestMeta(Id id, IUnit unit) : base(unit)
                => Id = id;

            public override Id Id { get; }
        }
    }
}
