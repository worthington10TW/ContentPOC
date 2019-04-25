using ContentPOC.Unit;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ContentPOC.Test.Unit
{
    public class NewsTests
    {
        protected static NewsItem _news => new NewsItem
        {
            new Headline("STUFF"),
            new StorySummary("things"),
            new StoryText("And stories")
        };

        [Fact]
        public void WhenNewsGettingUrl_ShouldReturnHashCode() =>
            _news.Meta.Href.Should().Be("newsitem/A357D733");
        
        //TODO unit type needs to follow URL convention i.e story-text
        [Fact]
        public void WhenHeadinlineGettingUrl_ShouldReturnHashCode() =>
            _news[0].Meta.Href.Should().Be("headline/A357D733");

        [Fact]
        public void WhenSummaryGettingUrl_ShouldReturnHashCode() =>
            _news[1].Meta.Href.Should().Be("storysummary/A357D733");

        [Fact]
        public void WhenStoryGettingUrl_ShouldReturnHashCode() =>
            _news[2].Meta.Href.Should().Be("storytext/A357D733");
    }
}