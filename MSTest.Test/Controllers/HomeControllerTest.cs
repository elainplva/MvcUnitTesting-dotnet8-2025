using Telerik.JustMock;
using MvcUnitTesting_dotnet8.Models;
using MvcUnitTesting_dotnet8.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace MvcUnitTesting.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Returns_All_books_In_DB()
        {
            //Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            Mock.Arrange( () => bookRepository.GetAll()).
                Returns(new List<Book>()
                {
                    new Book { Genre="Fiction", ID=1, Name="Moby Dick", Price=12.50m},
                    new Book { Genre="Fiction", ID=2, Name="War and Peace", Price=17m},
                    new Book { Genre="Science Fiction", ID=1, Name="Escape from the vortex", Price=12.50m},
                    new Book { Genre="History", ID=2, Name="The Battle of the Somme", Price=22m},
                }).MustBeCalled();

            //Act
            HomeController controller = new HomeController(bookRepository,null);
            ViewResult viewResult = controller.Index() as ViewResult;
            var model = viewResult.Model as IEnumerable<Book>;

            //Assert
            Assert.AreEqual(4, model.Count());

        }


        [TestMethod]
        public void show_ViewData_genre_test()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            Mock.Arrange(() => bookRepository.GetAll()).
                Returns(new List<Book>()
                {
                    new Book { Genre="Fiction", ID=1, Name="Moby Dick", Price=12.50m},
                    new Book { Genre="Fiction", ID=2, Name="War and Peace", Price=17m},
                    new Book { Genre="Science Fiction", ID=3, Name="Escape from the vortex", Price=12.50m},
                }).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            ViewResult result = controller.Index("Fiction") as ViewResult;

            // Assert
            Assert.AreEqual("Fiction", result.ViewData["Genre"]);
        }

        [TestMethod]
        public void test_book_by_genre()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            Mock.Arrange(() => bookRepository.GetAll()).
                Returns(new List<Book>()
                {
                    new Book { Genre="Fiction", ID=1, Name="Moby Dick", Price=12.50m},
                    new Book { Genre="Fiction", ID=2, Name="War and Peace", Price=17m},
                    new Book { Genre="Science Fiction", ID=3, Name="Escape from the vortex", Price=12.50m},
                    new Book { Genre="History", ID=4, Name="The Battle of the Somme", Price=22m},
                }).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            ViewResult result = controller.Index("Fiction") as ViewResult;
            var model = result.Model as IEnumerable<Book>;

            // Assert
            // Verify that only Fiction books are returned (count should be 2)
            Assert.AreEqual(2, model.Count());
            // Verify all returned books are Fiction genre
            Assert.IsTrue(model.All(b => b.Genre == "Fiction"));
        }

        [TestMethod]
        public void Privacy()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(bookRepository,null);

            // Act
            ViewResult result = controller.Privacy() as ViewResult;

            // Assert
            Assert.AreEqual("Your Privacy is our concern", result.ViewData["Message"]);
        }

        // ===== Tests for BooksByGenre Action =====
        [TestMethod]
        public void BooksByGenre_Returns_Books_For_Fiction()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var fictionBooks = new List<Book>()
            {
                new Book { Genre="Fiction", ID=1, Name="Moby Dick", Price=12.50m},
                new Book { Genre="Fiction", ID=2, Name="War and Peace", Price=17m}
            };

            Mock.Arrange(() => bookRepository.Find(Arg.IsAny<System.Linq.Expressions.Expression<System.Func<Book, bool>>>()))
                .Returns(fictionBooks).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            ViewResult result = controller.BooksByGenre("Fiction") as ViewResult;
            var model = result.Model as IEnumerable<Book>;

            // Assert
            Assert.AreEqual(2, model.Count());
            Assert.AreEqual("Fiction", result.ViewData["Genre"]);
        }

        [TestMethod]
        public void BooksByGenre_Returns_BadRequest_When_Genre_Empty()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(bookRepository, null);

            // Act
            BadRequestObjectResult result = controller.BooksByGenre("") as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void BooksByGenre_Returns_BadRequest_When_Genre_Null()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(bookRepository, null);

            // Act
            BadRequestObjectResult result = controller.BooksByGenre(null) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // ===== Tests for GenreCount Action =====
        [TestMethod]
        public void GenreCount_Returns_Correct_Count()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var scienceBooks = new List<Book>()
            {
                new Book { Genre="Science Fiction", ID=3, Name="Escape from the vortex", Price=12.50m}
            };

            Mock.Arrange(() => bookRepository.Find(Arg.IsAny<System.Linq.Expressions.Expression<System.Func<Book, bool>>>()))
                .Returns(scienceBooks).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            ViewResult result = controller.GenreCount("Science Fiction") as ViewResult;

            // Assert
            Assert.AreEqual(1, (int)result.ViewData["Count"]);
            Assert.AreEqual("Science Fiction", result.ViewData["Genre"]);
        }

        [TestMethod]
        public void GenreCount_Returns_BadRequest_When_Genre_Empty()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(bookRepository, null);

            // Act
            BadRequestObjectResult result = controller.GenreCount("") as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        // ===== Tests for GetBook Action =====
        [TestMethod]
        public void GetBook_Returns_Book_When_Exists()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            var book = new Book { ID = 1, Name = "Moby Dick", Genre = "Fiction", Price = 12.50m };

            Mock.Arrange(() => bookRepository.Get(1))
                .Returns(book).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            ViewResult result = controller.GetBook(1) as ViewResult;
            var model = result.Model as Book;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, model.ID);
            Assert.AreEqual("Moby Dick", model.Name);
        }

        [TestMethod]
        public void GetBook_Returns_NotFound_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();

            Mock.Arrange(() => bookRepository.Get(999))
                .Returns((Book)null).MustBeCalled();

            HomeController controller = new HomeController(bookRepository, null);

            // Act
            NotFoundResult result = controller.GetBook(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

    }
}
