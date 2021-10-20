using BookStore.Database.Entities;
using BookStore.Model.Books.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace BookStore.Model.Books.Interfaces
{
    public interface IBookService
    {
        public Task<List<Book>> GetAllBooksAsync();
        public Task<Book> GetBookByIdAsync(int? id);
        public List<SelectListItem> GetAuthorsSelectListItem();
        public Task CreateBookAsync(CreateBookRequest request);
        public Task UpdateBookAsync(UpdateBookRequest request);
        public Task DeleteBookAsync(int id);
    }
}
