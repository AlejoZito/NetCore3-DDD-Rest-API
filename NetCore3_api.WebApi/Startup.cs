using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Infrastructure;
using NetCore3_api.Infrastructure.Repositories;
using NetCore3_api.WebApi.SwaggerExamples;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

namespace NetCore3_api.WebApi
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
            services.AddControllers()
                    .AddNewtonsoftJson(); //Replace System.Json formatter with NewtonsoftJson
            //Migrations
            //dotnet ef migrations add Init --project NetCore3_api.Infrastructure --startup-project NetCore3_api.WebApi
            //dotnet ef database update --project NetCore3_api.Infrastructure --startup-project NetCore3_api.WebApi
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("NetCore3_api.Infrastructure")));

            services.AddScoped<IRepository<Charge>, ChargeRepository>();
            services.AddScoped<IRepository<EventType>, EventTypeRepository>();
            services.AddScoped<IRepository<Invoice>, InvoiceRepository>();
            services.AddScoped<IRepository<Payment>, PaymentRepository>();
            services.AddScoped<IRepository<User>, BaseRepository<User>>();

            //services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
            }, typeof(Startup));

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Meli API", Version = "v1" });
                // Enable Swagger examples
                c.ExampleFilters();
            });
            //Register swagger Examples
            services.AddSwaggerExamplesFromAssemblyOf<CreateEventRequestExample>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meli API V1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
