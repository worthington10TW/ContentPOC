using AutoFixture;
using ContentPOC.DAL;
using ContentPOC.Model;
using ContentPOC.Unit.Model;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ContentPOC.Integration
{
    public class InMemoryStoreTests : IDisposable
    {
        private readonly TestServer _testServer;
        private readonly InMemoryStore _store;
        private readonly Fixture _fixture = new Fixture();

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
                Meta = new TestMeta(id),
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
                Meta = new TestMeta(id),
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
                Meta = new TestMeta(new Id("amazingUnit")),
                Value = "i-live/here"
            });
            var toReplace = new TestUnit
            {
                Meta = new TestMeta(new Id("amazingUnit")),
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
                Meta = new TestMeta(id),
                Value = "i-live/here"
            };
            await _store.SaveAsync(unit);

            _store.Get(Guid.NewGuid().ToString(), id).Should().BeNull();
        }

        [Fact]
        public async Task WhenCallingGetllAll_ShouldGetAllUnderArea()
        {
            var unit1 = CreateUnits("cool-stuff", 20);
            var unit2 = CreateUnits("bad-things", 10);
            await Task.WhenAll(unit1.Concat(unit2).Select(u => _store.SaveAsync(u)));

            var results = _store.GetAll("cool-stuff");

            results.Count.Should().Be(20);
            results.Any(x => x.Namespace == "bad-things").Should().BeFalse();
            results.Any(x => x.Namespace == "cool-stuff").Should().BeTrue();
        }

        private IEnumerable<TestUnit> CreateUnits(string area, int count)
        {
            return Enumerable.Range(0, count).Select(i =>
            _fixture.Build<TestUnit>()
            .With(x => x.Namespace, area)
            .With(x => x.Meta, new TestMeta(new Id(Guid.NewGuid().ToString())))
            .With(x => x.Children, new List<IUnit>()).Create());
        }

        public void Dispose()
        {
            _store.Reset();
            _testServer.Dispose();
        }

        // TODO: tightly coupling ID to hash causes an excessively rigid codebase.  Decouple
        public class TestUnit : IUnit
        {
            public string Namespace { get; set; } = nameof(TestUnit);

            public IMeta Meta { get; set; }

            public string Value { get; set; }

            public List<IUnit> Children { get; set; } = new List<IUnit>();
        }

        public class TestMeta : IMeta
        {
            public TestMeta(Id id)
                => Id = id;

            public Id Id { get; }

            public Area Area => new Area();

            public string Href => string.Empty;
        }
    }
}
