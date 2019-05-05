using System;
using System.IO;
using System.Xml;
using ContentPOC.Converter;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Xunit;

namespace ContentPOC.Test
{
    public class NewsConverterTests
    {
        private readonly NewsItem _item;

        public NewsConverterTests()
        {
            var converter = new NewsConverter(new DynamicNamespaceManager());
            var xdoc = new XmlDocument();
            xdoc.LoadXml(File.ReadAllText($"{nameof(NewsConverterTests)}.xml"));
            _item = converter.Create(xdoc) as NewsItem;
        }

        [Fact]
        public void ShouldThrowWhenXmlIsNull()
        {
            Action action = () =>
            new NewsConverter(new DynamicNamespaceManager()).Create(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldNotBeNull() => _item.Should().NotBeNull();

        [Fact]
        public void ShouldHaveCorrectNumberOfChildren() =>
            _item.Children.Count.Should().Be(3);

        [Fact]
        public void ShouldHaveCorrectMeta()
        {
            _item.Meta.Id.Should().Be(new Guid("5b7d5c7c-f57f-5b59-834d-2e3394f5f8ae"));
            _item.Meta.Href.Should().Be("news/5b7d5c7c-f57f-5b59-834d-2e3394f5f8ae");
            }

    [Fact]
        public void ChildShouldHaveCorrectIdSet()
        {
            _item.Children[0].Meta.Id.Should().Be(new Guid("991be316-9f9d-9fd1-dbd2-9797088cb239"));
            _item.Children[0].Meta.Href.Should().Be("news/headlines/991be316-9f9d-9fd1-dbd2-9797088cb239");
            _item.Children[1].Meta.Id.Should().Be(new Guid("aa51c127-88a4-9c04-7c87-4366c09e64dd"));
            _item.Children[1].Meta.Href.Should().Be("news/story-summaries/aa51c127-88a4-9c04-7c87-4366c09e64dd");
            _item.Children[2].Meta.Id.Should().Be(new Guid("cb23f5ee-6d53-047e-4f75-b3cb517b43b3"));
            _item.Children[2].Meta.Href.Should().Be("news/story-text/cb23f5ee-6d53-047e-4f75-b3cb517b43b3");
        }
    }
}