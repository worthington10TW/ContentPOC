// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc;

namespace ContentPOC
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
