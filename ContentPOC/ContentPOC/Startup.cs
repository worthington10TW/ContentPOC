// /*----------------------------------------------------------------------------------------------*/
// /*                                                                                              */
// /*    Copyright © 2017 LexisNexis.  All rights reserved.                                        */
// /*    RELX Group plc trading as LexisNexis - Registered in England - Number 2746621.            */
// /*    Registered Office 1 - 3 Strand, London WC2N 5JR.                                          */
// /*                                                                                              */
// /*----------------------------------------------------------------------------------------------*/

using System;
using ContentPOC.Converter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;

namespace ContentPOC
{
    public class Startup
    {
	    private readonly IConfiguration _configuration;

	    public Startup(IConfiguration configuration)
	    {
		    _configuration = configuration;
	    }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");
            services.AddMetrics();
            services.AddMvc().AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                options.OperationFilter<SwaggerDefaultValues>();           
            });
            services
                .AddSingleton(_configuration)
                .AddSingleton<IConverter<Unit.News>>(x => new NewsConverter())            
                .AddSingleton<NewsConverter>();

            services.AddHealthChecks(checks =>
            {
                checks.AddCheck("sample", () => HealthCheckResult.Healthy("All's well"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }

            app.UseMiddleware<HealthCheckMiddleware>("/health", TimeSpan.FromSeconds(3));

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });            

            app.UseMvc();
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"ContentPOC API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "ContentPOC API.",
            };
            
            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
