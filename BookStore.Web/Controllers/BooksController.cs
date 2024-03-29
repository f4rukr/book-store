﻿using AutoMapper;
using BookStore.Database.Entities;
using BookStore.Model.Books.Interfaces;
using BookStore.Model.Books.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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


        public async Task<IActionResult> Index(string sortOrder, string searchFilter, string currentFilter, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchFilter != null)
                    pageNumber = 1;
                else
                    searchFilter = currentFilter;

                ViewData["CurrentFilter"] = searchFilter;

                return View(await _bookService.GetBooksAsync(sortOrder, searchFilter, pageNumber).ConfigureAwait(false));
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

        public async Task<IActionResult> Create()
        {
            try
            {
                var CreateUpdateBookRequest = new CreateBookRequest()
                {
                    ddlAuthors = await _bookService.GetAuthorsSelectListItem().ConfigureAwait(false)
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

                book.ddlAuthors = await _bookService.GetAuthorsSelectListItem().ConfigureAwait(false);
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

                var book = await _bookService.GetBookByIdAsync(id).ConfigureAwait(false);
                var bookRequest = _mapper.Map<Book, UpdateBookRequest>(book);

                bookRequest.SelectedAuthorIds = new List<SelectListItem>();
                foreach (var author in book.BookAuthors)
                    bookRequest.SelectedAuthorIds.Add(new SelectListItem() { Value = author.AuthorId.ToString() });

                bookRequest.ddlAuthors = await _bookService.GetAuthorsSelectListItem().ConfigureAwait(false);

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

                request.ddlAuthors = await _bookService.GetAuthorsSelectListItem().ConfigureAwait(false);
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

        public async Task<IActionResult> GroupBooksByPublishedDate()
        {
            try
            {
                return View(await _bookService.GroupBooksByPublishDate());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GroupBooksByPublishedDate));
                throw;
            }
        }
    }
}
