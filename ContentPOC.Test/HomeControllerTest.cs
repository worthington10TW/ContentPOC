// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ContentPOC.Test
{
    public class HomeControllerTest
    {
        [Fact]
        public void ShouldReturnRedirect()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<RedirectResult>(result);
            var content = (RedirectResult)result;
            Assert.Equal("/swagger", content.Url);
        }
    }
}
