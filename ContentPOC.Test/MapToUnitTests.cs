using System;
using System.Collections.Generic;
using System.Linq;
using ContentPOC.Extensions;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Xunit;
using static ContentPOC.Extensions.IUnitExtensions;

namespace ContentPOC.Test
{
    public class MapToUnitTests
    {
        private readonly Guid _id = Guid.NewGuid();

        [Fact]
        public void WhenAreaIsNull_ShouldThrow()
        {
            string[] area = null;

            Action action = () => area.ToUnit(_id, new Dictionary<string, object>());

            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\nParameter name: area");
        }

        [Fact]
        public void WhenDataIsNull_ShouldThrow()
        {
            string[] area = { "some", "area" };

            Action action = () => area.ToUnit(_id, null);

            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\nParameter name: data");
        }

        [Fact]
        public void WhenIdIsEmpty_ShouldThrow()
        {
            string[] area = { "some", "area" };

            Action action = () => area.ToUnit(Guid.Empty, new Dictionary<string, object>());

            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\nParameter name: id");
        }

        [Fact]
        public void WhenTypeIsHeadline_ShouldMapToHeadline()
        {
            var data = new Dictionary<string, object> { { "Value", "thing" } };
            var unit = new Headline("").Domain.ToUnit(_id, data);

            var headline = unit.Should().BeOfType<Headline>().Subject;
            headline.Value.Should().Be("thing");
            headline.Meta.Id.Should().Be(_id);
        }

        [Fact]
        public void WhenTypeIsStory_ShouldMapToStory()
        {
            var data = new Dictionary<string, object> { { "Value", "story" } };
            var unit = new StoryText("").Domain.ToUnit(_id, data);

            var storyText = unit.Should().BeOfType<StoryText>().Subject;
            storyText.Value.Should().Be("story");
            storyText.Meta.Id.Should().Be(_id);
        }

        [Fact]
        public void WhenTypeIsSummary_ShouldMapToSummary()
        {
            var data = new Dictionary<string, object> { { "Value", "summary" } };

            var unit = new StorySummary("").Domain.ToUnit(_id, data);

            var storySummary = unit.Should().BeOfType<StorySummary>().Subject;
            storySummary.Value.Should().Be("summary");
            storySummary.Meta.Id.Should().Be(_id);
        }

        [Fact]
        public void WhenTypeIsNews_ShouldMapToNews()
        {
            var childIds = Enumerable.Range(0, 10).Select(x => Guid.NewGuid()).ToArray();
            var data = new Dictionary<string, object> { { "Children", childIds } };

            var unit = new NewsItem().Domain.ToUnit(_id, data);

            var news = unit.Should().BeOfType<NewsItem>().Subject;
            news.ChildIds.Should().BeEquivalentTo(childIds);
            news.Meta.Id.Should().Be(_id);
        }
    }
}