using Microsoft.AspNetCore.Mvc;
using MvcUnitTesting_dotnet8.Models;
using System.Collections.Generic;

namespace MvcUnitTesting_dotnet8.Controllers
{
    public class BooksController : Controller
    {
        private readonly IRepository<Book> _repository;

        public BooksController(IRepository<Book> repository)
        {
            _repository = repository;
        }

        // GET: Books
        // Displays all books from the repository
        public IActionResult Index()
        {
            var books = _repository.GetAll();
            return View(books);
        }

        // GET: Books/Details/5
        // Displays details of a specific book by ID
        public IActionResult Details(int id)
        {
            var book = _repository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        // Displays the create book form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // Adds a new book to the repository
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        // Displays the edit book form
        public IActionResult Edit(int id)
        {
            var book = _repository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // Updates an existing book in the repository
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _repository.Update(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        // Displays the delete confirmation
        public IActionResult Delete(int id)
        {
            var book = _repository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        // Deletes a book from the repository
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _repository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            _repository.Remove(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
