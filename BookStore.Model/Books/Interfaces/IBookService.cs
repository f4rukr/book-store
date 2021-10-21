using BookStore.Database.Entities;
using BookStore.Model.Books.Request;
using BookStore.Model.Books.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model.Books.Interfaces
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooksAsync(string sortOrder, string searchFilter, int? pageNumber);
        public Task<Book> GetBookByIdAsync(int? id);
        public Task<List<SelectListItem>> GetAuthorsSelectListItem();
        public Task CreateBookAsync(CreateBookRequest request);
        public Task UpdateBookAsync(UpdateBookRequest request);
        public Task DeleteBookAsync(int id);
        public Task<List<GroupBooksByDate>> GroupBooksByPublishDate();
    }
}
