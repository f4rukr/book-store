using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        // GET: BooksController
        public ActionResult Index()
        {
            return View();
        }
    }
}
