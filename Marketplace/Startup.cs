using System.Threading.Tasks;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            const string connectionString = "Host=localhost;Database=marketplace;Username=ddd;Password=book";
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ClassifiedAdDbContext>(
                    options => options.UseNpgsql((connectionString)));
            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddScoped<ClassifiedAdsApplicationService>();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.EnsureDatabase();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

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