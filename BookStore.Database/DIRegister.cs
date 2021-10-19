using BookStore.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace BookStore.Database
{
    public static class DIRegister
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContextPool<BookStoreDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("BookStoreDbContext")));
        }
    }
}
