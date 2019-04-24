// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace ContentPOC.Integration
{
    public class ApplicationShould : IDisposable
    {
        private readonly TestServer _server = new TestServer(Program.WebHostBuilder(null));
        private readonly HttpClient _client;

        public ApplicationShould()
        {
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        [Fact]
        public async Task ShouldReturnHealthy_WhenHittingHealthEndpoint()
        {
            var result = await _client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultAsHealth = await result.Content.ReadAsAsync<Health>();
            resultAsHealth.Status.Should().Be("Healthy");
        }

        [Fact]
        public async Task ReturnGreetingInJson()
        {
            var response = await _client.GetAsync("/api/v1/greeting");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var definition = new {Greeting = "", Greetee = ""};
            var parsedResponse = JsonConvert.DeserializeAnonymousType(responseString, definition);

            parsedResponse.Greeting.Should().Equals("Hello");
            parsedResponse.Greetee.Should().Equals("World!");
        }

        [Fact]
        public async Task ReturnGreetingInText()
        {
            var response = await _client.GetAsync("/api/v2/greeting");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().Equals("Hello World!");
        }

        public class Health
        {
            public string Status { get; set; }
        }
    }
}
