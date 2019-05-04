using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using ContentPOC.NewsIngestor;
using ContentPOC.Seed;
using ContentPOC.Unit.Model.News;
using FluentAssertions;
using Moq;
using Xunit;

namespace ContentPOC.Integration
{
    public abstract class SeedTests
    {
        protected readonly XmlSeeder _xmlSeeder;
        protected readonly Mock<IManager<NewsItem>> _mockManager =
            new Mock<IManager<NewsItem>>();

        protected SeedTests() =>
            _xmlSeeder = new XmlSeeder(_mockManager.Object);
    }

    public class WhenSeedingData : SeedTests
    {
        public WhenSeedingData() =>
             _xmlSeeder.SeedAsync(Path.Combine("SeedData"))
                .GetAwaiter()
                .GetResult();

        [Fact]
        public void ShouldIngestAllXml() 
        {
            _mockManager.Verify(x => x.SaveAsync(
                    It.Is<XmlDocument>(y => y.SelectSingleNode("/test").InnerXml == "yep")),
                    Times.Once);
            _mockManager.Verify(x => x.SaveAsync(
                    It.Is<XmlDocument>(y => y.SelectSingleNode("/test").InnerXml == "yep1")),
                    Times.Once);
        }

        [Fact]
        public void ShouldNotIngestOtherFileTypes()
        {
            _mockManager.Verify(x => x.SaveAsync(
                       It.Is<XmlDocument>(y => y.SelectSingleNode("/test").InnerXml == "nope")),
                       Times.Never);
        }
    }

    public class WhenCannotIngest : SeedTests
    {
        public WhenCannotIngest()
        {
            _mockManager.Setup(x => x.SaveAsync(It.IsAny<XmlDocument>()))
                .Throws<Exception>();
        }

        [Fact]
        public void ShouldNotThrow()
        {
            Func<Task> fun = () => _xmlSeeder.SeedAsync(Path.Combine("SeedData"));
            fun.Should().NotThrow<Exception>();
        }

    }
}
