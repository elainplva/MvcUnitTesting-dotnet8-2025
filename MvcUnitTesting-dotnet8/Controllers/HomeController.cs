using Microsoft.AspNetCore.Mvc;
using MvcUnitTesting_dotnet8.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Tracker;
using Tracker.WebAPIClient;

namespace MvcUnitTesting_dotnet8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository<Book> repository;

        public HomeController(IRepository<Book> bookRepo, ILogger<HomeController> logger)
        {
            ActivityAPIClient.Track(StudentID: "s00250500", StudentName: "Elain Polakova",
                activityName: "Rad302 2026 Week 2 Lab 1", Task: "Running initial tests");

            repository = bookRepo;
            _logger = logger;
        }
        
        public IActionResult Index(string genre = null)
        {
            var books = repository.GetAll();

            if (!string.IsNullOrEmpty(genre))
            {
                ViewData["Genre"] = genre;
                books = books.Where(b => b.Genre == genre).ToList();
            }

            return View(books);
        }

        // Get books by genre using Find method from repository
        public IActionResult BooksByGenre(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest("Genre parameter is required");
            }

            var books = repository.Find(b => b.Genre == genre);
            ViewData["Genre"] = genre;
            return View("Index", books);
        }

        // Get book count for a specific genre
        public IActionResult GenreCount(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest("Genre parameter is required");
            }

            var books = repository.Find(b => b.Genre == genre).ToList();
            ViewData["Genre"] = genre;
            ViewData["Count"] = books.Count();
            return View(books);
        }

        // Get a single book by ID
        public IActionResult GetBook(int id)
        {
            var book = repository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Your Privacy is our concern";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
