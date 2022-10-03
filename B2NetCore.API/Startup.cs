using B2Net.Client;
using B2Net.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace B2NetCore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = FileSize.Max;
            });

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = FileSize.Max;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BackBlaze B2 API",
                    Version = "v1",
                    Description = "This is a File service API",
                    Contact = new OpenApiContact
                    {
                        Name = "Lior Brand",
                        Email = string.Empty,
                    },
                });
            });

            services.Configure<B2Settings>(Configuration.GetSection("B2Settings"));
            services.AddScoped<IB2NetClient>(sp => new B2NetClient(GetSettings()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackBlaze B2 API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private B2Settings GetSettings()
        {
            return new B2Settings
            {
                ApplicationKey = Configuration.GetSection("B2Settings:ApplicationKey").Value,
                KeyId = Configuration.GetSection("B2Settings:KeyId").Value,
            };
        }
    }
}
