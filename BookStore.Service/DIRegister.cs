using BookStore.Model.Books.Interfaces;
using BookStore.Service.Books;
using Microsoft.Extensions.DependencyInjection;
namespace BookStore.Service
{
    public static class DIRegister
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
        }
    }
}
