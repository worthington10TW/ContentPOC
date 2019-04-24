// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContentPOC
{
    public class Greet
    {
        public string Greeting { get; }
        public string Greetee { get; }

        public Greet(string greeting, string greetee)
        {
            Greeting = greeting;
            Greetee = greetee;
        }
    }

    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/greeting")]
    public class GreetingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GreetingController> _logger;

        public GreetingController(IConfiguration configuration, ILogger<GreetingController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var greetee = _configuration.GetValue("greetee", "Penguin!");
            var greeting = new Greet("Hello", greetee);
            _logger.LogInformation("The greeting is {@greeting}", greeting);
            return Ok(greeting);
        }
    }

    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/greeting")]
    public class GreetingController2 : Controller
    {
        private readonly IConfiguration _configuration;

        public GreetingController2(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var greetee = _configuration.GetValue("greetee", "Penguin!");
            return Ok("Hello " + greetee);
        }
    }
}
