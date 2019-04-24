﻿using ContentPOC.Converter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ContentPOC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>())
                .AddMvc()
                .AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters();

            services.AddMvcCore();

            services
                .AddSingleton<IConverter<Unit.News>>(x => new NewsConverter())
                .AddSingleton<NewsConverter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            (env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseStatusCodePages())
                .UseStaticFiles()
                .UseSwagger()
                .UseSwaggerUI()
                .UseMvc();
        }
    }
}
