using ContentPOC.Unit;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ContentPOC.Test.Unit
{
    public class NewsTests
    {
        protected static News _news => new News
        {
            new Headline("STUFF"),
            new Summary("things"),
            new Story("And stories")
        };

        [Fact]
        public void WhenNewsGettingUrl_ShouldReturnHashCode() =>
            _news.Meta.Href.Should().Be("news/17867F64");
        
        [Fact]
        public void WhenHeadinlineGettingUrl_ShouldReturnHashCode() =>
            _news[0].Meta.Href.Should().Be("headline/17867F64");

        [Fact]
        public void WhenSummaryGettingUrl_ShouldReturnHashCode() =>
            _news[1].Meta.Href.Should().Be("summary/17867F64");

        [Fact]
        public void WhenStoryGettingUrl_ShouldReturnHashCode() =>
            _news[2].Meta.Href.Should().Be("story/17867F64");
    }
}