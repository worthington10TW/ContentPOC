// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContentPOC.Test
{
    public class GreetingControllerTest
    {
        [Fact]
        public void ShouldReturnHelloSailor()
        {
            var configurationDictionary = new Dictionary<string, string>
            {
                {"greetee", "Sailor!"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationDictionary)
                .Build();
            var logger = Mock.Of<ILogger<GreetingController>>();
            var controller = new GreetingController(configuration, logger);

            var result = controller.Index();

            Assert.IsType<OkObjectResult>(result);
            var content = (OkObjectResult)result;
            var value = (Greet)content.Value;
            Assert.Equal("Hello", value.Greeting);
            Assert.Equal("Sailor!", value.Greetee);
        }
    }
}
