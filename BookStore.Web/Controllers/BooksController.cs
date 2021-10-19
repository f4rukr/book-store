using BookStore.Model.Books.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookService bookService,
            ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _bookService.GetAllBooksAsync().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET:/Books");
                throw;
            }
        }
    }
}
