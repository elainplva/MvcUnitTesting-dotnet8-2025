using Telerik.JustMock;
using MvcUnitTesting_dotnet8.Models;
using MvcUnitTesting_dotnet8.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MvcUnitTesting.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        // ===== Tests for Index Action =====
        [TestMethod]
        public void Index_Returns_All_Books()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var books = new List<Book>()
            {
                new Book { ID=1, Name="Moby Dick", Genre="Fiction", Price=12.50m},
                new Book { ID=2, Name="War and Peace", Genre="Fiction", Price=17m},
                new Book { ID=3, Name="Escape from the vortex", Genre="Science Fiction", Price=12.50m},
            };
            
            Mock.Arrange(() => bookRepository.GetAll())
                .Returns(books).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var model = result.Model as IEnumerable<Book>;

            // Assert
            Assert.AreEqual(3, model.Count());
        }

        // ===== Tests for Details Action =====
        [TestMethod]
        public void Details_Returns_Book_When_Exists()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 1, Name = "Moby Dick", Genre = "Fiction", Price = 12.50m };
            
            Mock.Arrange(() => bookRepository.Get(1))
                .Returns(book).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            ViewResult result = controller.Details(1) as ViewResult;
            var model = result.Model as Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Moby Dick", model.Name);
        }

        [TestMethod]
        public void Details_Returns_NotFound_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            
            Mock.Arrange(() => bookRepository.Get(999))
                .Returns((Book)null).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            NotFoundResult result = controller.Details(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        // ===== Tests for Create GET Action =====
        [TestMethod]
        public void Create_Get_Returns_View()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            BooksController controller = new BooksController(bookRepository);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);
        }

        // ===== Tests for Create POST Action =====
        [TestMethod]
        public void Create_Post_Adds_Book_And_Redirects()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var newBook = new Book { ID = 5, Name = "New Book", Genre = "Drama", Price = 15.00m };
            
            Mock.Arrange(() => bookRepository.Add(newBook))
                .DoNothing().MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            RedirectToActionResult result = controller.Create(newBook) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Mock.Assert(bookRepository);
        }

        // ===== Tests for Edit GET Action =====
        [TestMethod]
        public void Edit_Get_Returns_Book_When_Exists()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 2, Name = "War and Peace", Genre = "Fiction", Price = 17m };
            
            Mock.Arrange(() => bookRepository.Get(2))
                .Returns(book).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            ViewResult result = controller.Edit(2) as ViewResult;
            var model = result.Model as Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.ID);
        }

        [TestMethod]
        public void Edit_Get_Returns_NotFound_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            
            Mock.Arrange(() => bookRepository.Get(999))
                .Returns((Book)null).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            NotFoundResult result = controller.Edit(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // ===== Tests for Edit POST Action =====
        [TestMethod]
        public void Edit_Post_Updates_Book_And_Redirects()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var updatedBook = new Book { ID = 1, Name = "Moby Dick Updated", Genre = "Fiction", Price = 13.00m };
            
            Mock.Arrange(() => bookRepository.Update(updatedBook))
                .DoNothing().MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            RedirectToActionResult result = controller.Edit(1, updatedBook) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Edit_Post_Returns_BadRequest_When_ID_Mismatch()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 1, Name = "Moby Dick", Genre = "Fiction", Price = 12.50m };
            
            BooksController controller = new BooksController(bookRepository);

            // Act
            BadRequestResult result = controller.Edit(2, book) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        // ===== Tests for Delete GET Action =====
        [TestMethod]
        public void Delete_Get_Returns_Book_When_Exists()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 3, Name = "Escape from the vortex", Genre = "Science Fiction", Price = 12.50m };
            
            Mock.Arrange(() => bookRepository.Get(3))
                .Returns(book).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            ViewResult result = controller.Delete(3) as ViewResult;
            var model = result.Model as Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, model.ID);
        }

        [TestMethod]
        public void Delete_Get_Returns_NotFound_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            
            Mock.Arrange(() => bookRepository.Get(999))
                .Returns((Book)null).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            NotFoundResult result = controller.Delete(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // ===== Tests for Delete POST Action =====
        [TestMethod]
        public void DeleteConfirmed_Removes_Book_And_Redirects()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 1, Name = "Moby Dick", Genre = "Fiction", Price = 12.50m };
            
            Mock.Arrange(() => bookRepository.Get(1))
                .Returns(book).MustBeCalled();
            
            Mock.Arrange(() => bookRepository.Remove(book))
                .DoNothing().MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            RedirectToActionResult result = controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void DeleteConfirmed_Returns_NotFound_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            
            Mock.Arrange(() => bookRepository.Get(999))
                .Returns((Book)null).MustBeCalled();

            BooksController controller = new BooksController(bookRepository);

            // Act
            NotFoundResult result = controller.DeleteConfirmed(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
