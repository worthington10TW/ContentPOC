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
            _store.Get(new[] { "super-area" }, Guid.NewGuid())
                .Should()
                .BeNull();

        [Fact]
        public async Task WhenNewResult_ShouldInsert()
        {
            var id = Guid.NewGuid();
            var unit = new TestUnit
            {
                Meta = new TestMeta(id),
                Value = "i-live/here"
            };
            unit.Meta.SetId(id);

            await _store.SaveAsync(unit);

            _store.Get(unit.Domain, id).Should().BeEquivalentTo(unit);
        }

        [Fact]
        public async Task WhenClearing_ShouldBeEmpty()
        {
            var id = Guid.NewGuid();
            var unit = new TestUnit
            {
                Meta = new TestMeta(id),
                Value = "i-live/here"
            };
            unit.Meta.SetId(id);

            await _store.SaveAsync(unit);
            _store.Get(unit.Domain, id).Should().BeEquivalentTo(unit);
            _store.Reset();

            _store.Get(unit.Domain, id).Should().BeNull();
        }

        [Fact]
        public async Task WhenIdMatches_ShouldReplace()
        {
            var id = Guid.NewGuid();
            await _store.SaveAsync(new TestUnit
            {
                Meta = new TestMeta(id),
                Value = "i-live/here"
            });
            var toReplace = new TestUnit
            {
                Meta = new TestMeta(id),
                Value = "i-dont-look/the-same"
            };

            await _store.SaveAsync(toReplace);

            _store.Get(toReplace.Domain, id).Should().BeEquivalentTo(toReplace);
        }

        [Fact]
        public async Task WhenCallingWithNonMatchingArea_ShouldNotReturn()
        {
            var id = Guid.NewGuid();
            var unit = new TestUnit
            {
                Meta = new TestMeta(id),
                Value = "i-live/here"
            };
            await _store.SaveAsync(unit);

            _store.Get(new[] { Guid.NewGuid().ToString() }, id).Should().BeNull();
        }

        [Fact]
        public async Task WhenCallingGetllAll_ShouldGetAllUnderArea()
        {
            var unit1 = CreateUnits(new[] { "cool-stuff" }, 20);
            var unit2 = CreateUnits(new[] { "bad-things" }, 10);
            await Task.WhenAll(unit1.Concat(unit2).Select(u => _store.SaveAsync(u)));

            var results = _store.GetAll(new[] { "cool-stuff" });

            results.Count.Should().Be(20);
            results.Any(x => x.Domain[0] == "bad-things").Should().BeFalse();
            results.Any(x => x.Domain[0] == "cool-stuff").Should().BeTrue();
        }

        private IEnumerable<TestUnit> CreateUnits(string[] domain, int count)
        {
            return Enumerable.Range(0, count).Select(i =>
            _fixture.Build<TestUnit>()
            .With(x => x.Domain, domain)
            .With(x => x.Meta, new TestMeta(Guid.NewGuid()))
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
            public string[] Domain { get; set; } = new[] { nameof(TestUnit) };

            public IMeta Meta { get; set; }

            public string Value { get; set; }

            public List<IUnit> Children { get; set; } = new List<IUnit>();

            public List<IMark> Marks => new List<IMark>();
        }

        public class TestMeta : IMeta
        {
            public TestMeta(Guid id)
                => Id = id;

            public Guid Id { get; private set; }

            public Area Area => new Area(null);

            public string Href => string.Empty;

            public void SetId(Guid id) => Id = id;
        }
    }
}
