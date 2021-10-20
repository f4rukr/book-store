using AutoMapper;
using BookStore.Database.Entities;
using BookStore.Model.Books.Interfaces;
using BookStore.Model.Books.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        private readonly IMapper _mapper;
        public BooksController(
            IBookService bookService,
            IMapper mapper,
            ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _bookService.GetAllBooksAsync().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Index));
                throw;
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id).ConfigureAwait(false);

                if (book == null)
                    return NotFound();

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Details));
                throw;
            }
        }

        public IActionResult Create()
        {
            try
            {
                var CreateUpdateBookRequest = new CreateBookRequest()
                {
                    ddlAuthors = _bookService.GetAuthorsSelectListItem()
                };

                return View(CreateUpdateBookRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Create));
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookRequest book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _bookService.CreateBookAsync(book).ConfigureAwait(false);
                    return RedirectToAction(nameof(Index));
                }

                book.ddlAuthors = _bookService.GetAuthorsSelectListItem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Create));
                throw;
            }

            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var book = await _bookService.GetBookByIdAsync(id);
                var bookRequest = _mapper.Map<Book, UpdateBookRequest>(book);

                bookRequest.SelectedAuthorIds = new List<SelectListItem>();
                foreach (var author in book.BookAuthors)
                    bookRequest.SelectedAuthorIds.Add(new SelectListItem() { Value = author.AuthorId.ToString() });

                bookRequest.ddlAuthors = _bookService.GetAuthorsSelectListItem();

                return View(bookRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Edit));
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _bookService.UpdateBookAsync(request).ConfigureAwait(false);
                    return RedirectToAction(nameof(Index));
                }

                request.SelectedAuthorIds = new List<SelectListItem>();
                if (request.AuthorIds != null)
                    foreach (var authorId in request.AuthorIds)
                        request.SelectedAuthorIds.Add(new SelectListItem() { Value = authorId.ToString() });

                request.ddlAuthors = _bookService.GetAuthorsSelectListItem();
                return View("Edit", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Update));
                throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var book = await _bookService.GetBookByIdAsync(id);

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Delete));
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ConfirmDelete));
                throw;
            }
        }
    }
}
