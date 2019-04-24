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
            Headline = "STUFF",
            Summary = "things",
            Story = "And stories"
        };

        [Fact]
        public void WhenStringifyingObject_ShouldReturnValidJson() =>
            JsonConvert.DeserializeObject<News>(_news.ToString())
            .Should().BeEquivalentTo(_news);
        
        [Fact]
        public void WhenGettingUrl_ShouldReturnHashCode() =>
            _news.Href.Should().Be("news/E0B8D965");
    }
}
