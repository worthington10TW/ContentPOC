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
            Headline = new Headline("STUFF"),
            Summary = new Summary("things"),
            Story = new Story("And stories")
        };

        [Fact]
        public void WhenStringifyingObject_ShouldReturnValidJson() =>
            JsonConvert.DeserializeObject<News>(_news.ToString())
            .Should().BeEquivalentTo(_news);
        
        [Fact]
        public void WhenGettingUrl_ShouldReturnHashCode() =>
            _news.Href.Should().Be("news/9AD1869F");
    }
}
