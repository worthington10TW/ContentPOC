using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Xunit;

namespace ContentPOC.Test.Unit
{
    public class NewsTests
    {
        protected static NewsItem _news => new NewsItem(
            new Headline("STUFF"),
            new StorySummary("things"),
            new StoryText("And stories"));

        [Fact]
        public void WhenNewsGettingUrl_ShouldReturnHashCode() =>
            _news.Meta.Href.Should().Be("news/5D17988D");

        //TODO unit type needs to follow URL convention i.e story-text
        [Fact]
        public void WhenHeadinlineGettingUrl_ShouldReturnHashCode() =>
            _news.Children[0].Meta.Href.Should().Be("news/headlines/6080D015");

        [Fact]
        public void WhenSummaryGettingUrl_ShouldReturnHashCode() =>
            _news.Children[1].Meta.Href.Should().Be("news/story-summaries/71B4880A");

        [Fact]
        public void WhenStoryGettingUrl_ShouldReturnHashCode() =>
            _news.Children[2].Meta.Href.Should().Be("news/story-text/1FED68F1");
    }
}