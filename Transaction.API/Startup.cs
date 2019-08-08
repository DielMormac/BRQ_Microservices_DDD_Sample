using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transaction.API.Handlers;
using Transaction.Infra.Factory;
using Transaction.Infra.Repository;

namespace Transaction.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //Add DbContext
            services.AddDbContext<AccountRepository>
                (options => options.UseInMemoryDatabase("AccountRepository"));
            services.AddDbContext<AccountTransactionRepository>
                (options => options.UseInMemoryDatabase("AccountTransactionRepository"));
            //MediatR API
            services.AddMediatR(GetType().Assembly);
            //Dependency Injection
            services.AddScoped<RepositoryFactory>()
                .AddScoped<AccountsHandler>()
                .AddScoped<AccountTransactionsHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
