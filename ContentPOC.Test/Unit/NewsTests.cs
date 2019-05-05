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

        //[Fact]
        //public void WhenNewsGettingUrl_ShouldReturnHashCode() =>
        //    _news.ToGuid().ToString().Should().Be("c702e4f7-0b12-1f03-f231-44c950d0bb1a");

        ////TODO unit type needs to follow URL convention i.e story-text
        //[Fact]
        //public void WhenHeadinlineGettingUrl_ShouldReturnHashCode() =>
        //    _news.Children[0].ToGuid().ToString().Should().Be("976bb4f5-be6b-d1e0-a2bc-46a00e613c3f");

        //[Fact]
        //public void WhenSummaryGettingUrl_ShouldReturnHashCode() =>
        //    _news.Children[1].ToGuid().ToString().Should().Be("4762a65f-f8ae-d003-8fba-cfd86a230cd9");

        //[Fact]
        //public void WhenStoryGettingUrl_ShouldReturnHashCode() =>
            //_news.Children[2].ToGuid().ToString().Should().Be("b9f7c598-db5e-ee99-6f84-44dcd065d6a2");
    }
}