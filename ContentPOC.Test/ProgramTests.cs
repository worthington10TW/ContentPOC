using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ContentPOC.Test
{
    public class ProgramTests
    {
        [Fact]
        public async Task ShouldListenOnCorrectPorts()
        {
            var greetee = Guid.NewGuid().ToString();
            using (var host = Program.WebHostBuilder("--greetee", greetee).Build())
            {
                await host.StartAsync();
                using (var client = new HttpClient())
                {
                    var httpResult1 = await client.GetAsync("http://localhost:5000/api/v1/greeting");
                    var httpStringResult1 = await httpResult1.Content.ReadAsStringAsync();

                    var httpResult2 = await client.GetAsync("http://localhost:5001/api/v1/greeting");
                    var httpStringResult2 = await httpResult2.Content.ReadAsStringAsync();

                    Assert.Contains(greetee, httpStringResult1);
                    Assert.Contains(greetee, httpStringResult2);
                }

                await host.StopAsync();
            }

            Thread.Sleep(500);
        }

        [Fact]
        public async Task ShouldHaveMetricsEndpointsConfiguredToPrometheusTextFormat()
        {
            TestServer server = new TestServer(Program.WebHostBuilder(null));
            HttpClient client = server.CreateClient();

            var result = await client.GetAsync("/metrics");

            var content = await result.Content.ReadAsStringAsync();
            string error = null;
            try
            {
                JsonConvert.DeserializeObject(content);
            }
            catch(JsonReaderException e)
            {
                error = e.Message;
            }

            Assert.NotEmpty(error);

            Assert.StartsWith("# HELP application_httprequests_apdex", content);
        }

    }
}
