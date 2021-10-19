using AutoMapper;
using BookStore.Database.DbContexts;
using BookStore.Database.Entities;
using BookStore.Model.Books.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Books
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly ILogger<BookService> _logger;
        private readonly IMapper _mapper;

        public BookService(
            BookStoreDbContext dbContext,
            ILogger<BookService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<List<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _dbContext.Books.ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(BookService));
                throw;
            }
        }
    }
}
