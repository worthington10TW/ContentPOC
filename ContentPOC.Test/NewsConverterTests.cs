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
            _item.Meta.Id.Should().Be(new Guid("13f2a7e3-1272-d633-3e23-11c35d634bac"));
            _item.Meta.Href.Should().Be("news/13f2a7e3-1272-d633-3e23-11c35d634bac");
        }

        [Fact]
        public void FirstChildShouldHaveCorrectIdSet()
        {
            _item.Children[0].Meta.Id.Should().Be(new Guid("7a7ea619-f75f-27ca-ac5a-35165adc2516"));
            _item.Children[0].Meta.Href.Should().Be("news/headlines/7a7ea619-f75f-27ca-ac5a-35165adc2516");
        }

        [Fact]
        public void SecondChildShouldHaveCorrectIdSet()
        {
            _item.Children[1].Meta.Id.Should().Be(new Guid("900c8b4d-6d0f-2b9e-2c2f-fe6c662863b8"));
            _item.Children[1].Meta.Href.Should().Be("news/story-summaries/900c8b4d-6d0f-2b9e-2c2f-fe6c662863b8");
        }
        [Fact]
        public void ThirdChildShouldHaveCorrectIdSet()
        {
            _item.Children[2].Meta.Id.Should().Be(new Guid("52f3e494-e16c-5207-e7eb-0891ba36472e"));
            _item.Children[2].Meta.Href.Should().Be("news/story-text/52f3e494-e16c-5207-e7eb-0891ba36472e");
        }
    }
}