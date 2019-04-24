using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ContentPOC
{
    public class Program
    {
        public static void Main(string[] args) =>
            WebHostBuilder(args).Build().Run();

        public static IWebHostBuilder WebHostBuilder(params string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseDefaultServiceProvider(
                    (context, options) => 
                        options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .UseStartup<Startup>();
    }
}
