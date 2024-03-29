﻿using AutoMapper;
using BookStore.Database.DbContexts;
using BookStore.Database.Entities;
using BookStore.Model.Books.Interfaces;
using BookStore.Model.Books.Request;
using BookStore.Model.Books.Response;
using BookStore.Service.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            IMapper mapper,
            ILogger<BookService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<SelectListItem>> GetAuthorsSelectListItem()
        {
            try
            {
                var authorsListeItem = await _dbContext.Authors.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.LastName), Value = x.Id.ToString() })
                                                               .ToListAsync()
                                                               .ConfigureAwait(false);
                return authorsListeItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAuthorsSelectListItem));
                throw;
            }
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            try
            {
                var authors = await _dbContext.Authors.ToListAsync().ConfigureAwait(false);
                return authors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAllAuthorsAsync));
                throw;
            }
        }

        public async Task<List<Book>> GetBooksAsync(string sortOrder, string searchFilter, int? pageNumber)
        {
            try
            {
                var books = from b in _dbContext.Books
                               select b;

                if (!String.IsNullOrEmpty(searchFilter))
                    books = books.Where(s => s.Title.Contains(searchFilter));

                switch (sortOrder)
                {
                    case "title_desc":
                        books = books.OrderByDescending(s => s.Title);
                        break;
                    case "Date":
                        books = books.OrderBy(s => s.Published);
                        break;
                    case "date_desc":
                        books = books.OrderByDescending(s => s.Published);
                        break;
                    default:
                        books = books.OrderBy(s => s.Title);
                        break;
                }

                int pageSize = 3;
                return await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetBooksAsync));
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(int? id)
        {
            try
            {
                var book = await _dbContext.Books.Include(x => x.BookAuthors)
                                                 .ThenInclude(x => x.Author)
                                                 .Include(x => x.BookChanges)
                                                 .FirstOrDefaultAsync(x => x.Id == id)
                                                 .ConfigureAwait(false);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetBookByIdAsync));
                throw;
            }
        }

        public async Task CreateBookAsync(CreateBookRequest request)
        {
            try
            {
                var book = _mapper.Map<CreateBookRequest, Book>(request);

                book.BookAuthors = new List<BookAuthor>();

                foreach (var authorId in request.AuthorIds)
                    book.BookAuthors.Add(new BookAuthor() { BookId = book.Id, AuthorId = authorId });

                await _dbContext.Books.AddAsync(book).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateBookAsync));
                throw;
            }
        }

        public async Task UpdateBookAsync(UpdateBookRequest request)
        {
            try
            {
                var bookHistoryItem = new BookChange() { BookId = (int)request.Id, Reason = request.Reason };
                await _dbContext.BookChanges.AddAsync(bookHistoryItem).ConfigureAwait(false);

                //remove old authors
                var book = await GetBookByIdAsync(request.Id).ConfigureAwait(false);
                _dbContext.RemoveRange(book.BookAuthors);

                //add new authors
                var bookAuthors = new List<BookAuthor>();
                foreach (var authorId in request.AuthorIds)
                    bookAuthors.Add(new BookAuthor() { AuthorId = authorId, BookId = (int)request.Id });
                await _dbContext.AddRangeAsync(bookAuthors).ConfigureAwait(false);

                book.Title = request.Title;
                book.Description = request.Description;
                book.Published = request.Published;

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateBookAsync));
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                if (book != null) 
                {
                    _dbContext.Remove(book);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteBookAsync));
                throw;
            }
        }

        public async Task<List<GroupBooksByDate>> GroupBooksByPublishDate()
        {

            var result = _dbContext.Books
                                   .GroupBy( x => x.Published.Date, x => x.Title, (key, value) => 
                                    new GroupBooksByDate() { PublishedDate = key.Date, BookCount = value.Count() });

            return await result.AsNoTracking().ToListAsync();
        }
    }
}
