using AutoMapper;
using BookStore.Database;
using BookStore.Database.DbContexts;
using BookStore.Service;
using BookStore.Web.AutoMapper;
using BookStore.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.Web
{
    public class Startup
    {
        private MapperConfiguration _mapperConfig { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDatabase(Configuration);
            services.AddServices();
            services.AddAppInsights();
            services.AddSingleton(sp => _mapperConfig.CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BookStoreDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                dbContext.Database.Migrate();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAppInsights();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
