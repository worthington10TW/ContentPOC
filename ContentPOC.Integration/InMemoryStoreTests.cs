using ContentPOC.DAL;
using ContentPOC.Model;
using ContentPOC.Unit;
using ContentPOC.Unit.Model;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            _store.Get("super-area", new Id(Guid.NewGuid().ToString()))
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

            _store.Get(unit.Namespace, id).Should().BeEquivalentTo(unit);
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
            _store.Get(unit.Namespace, id).Should().BeEquivalentTo(unit);
            _store.Reset();

            _store.Get(unit.Namespace, id).Should().BeNull();
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

            _store.Get(toReplace.Namespace, new Id("amazingUnit")).Should().BeEquivalentTo(toReplace);
        }

        [Fact]
        public async Task WhenCallingWithNonMatchingArea_ShouldNotReturn()
        {
            var id = new Id("amazingUnit");
            var unit = new TestUnit
            {
                Meta = new TestMeta(id, new TestUnit()),
                Value = "i-live/here"
            };
            await _store.SaveAsync(unit);

            _store.Get(Guid.NewGuid().ToString(), id).Should().BeNull();
        }

        public void Dispose()
        {
            _store.Reset();
            _testServer.Dispose();
        }

        // TODO: tightly coupling ID to hash causes an excessively rigid codebase.  Decouple
        public class TestUnit : IUnit
        {
            public string Namespace => nameof(TestUnit);

            public Meta Meta { get; set; }

            public string Value { get; set; }

            public List<IUnit> Children { get; } = new List<IUnit>();
        }

        public class TestMeta : Meta
        {
            public TestMeta(Id id, IUnit unit) : base(unit)
                => Id = id;

            public override Id Id { get; }
        }
    }
}
