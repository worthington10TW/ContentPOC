using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;    
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using ContentPOC.Converter;
using System;

namespace ContentPOC.Integration
{
    public class SetupTests : IDisposable
    {
        private readonly IWebHostBuilder _builder = Program.WebHostBuilder();
        private readonly TestServer _server;

        public SetupTests() => _server = new TestServer(_builder);

        public void Dispose() => _server?.Dispose();

        [Fact]
        public void ShouldRegisterIConverterNews()
        {
            var converter = _server.Host.Services.GetService<IConverter<Unit.News>>();  
            
            converter.Should().NotBeNull();
            converter.Should().BeOfType<NewsConverter>();
        }

    }
}
