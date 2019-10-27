using System.Threading.Tasks;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;

namespace Marketplace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var store = new DocumentStore
            {
                Urls = new[] {"http://localhost:8080"},
                Database = "Marketplace_Chapter6",
                Conventions =
                {
                    FindIdentityProperty = m => m.Name == "_databaseId"
                }
            };
            store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>(
                (dbName, entity) => Task.FromResult("ClassifiedAd/" + entity.Id.ToString()));
            store.Initialize();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddTransient(c => store.OpenAsyncSession());
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
            services.AddSingleton<ClassifiedAdsApplicationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}