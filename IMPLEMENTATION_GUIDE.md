# Book Controller and Repository Pattern Implementation

## Overview
This document outlines the complete implementation of a Book Controller using the Repository Pattern, along with comprehensive unit tests for testing strategies.

## Architecture Components

### 1. **IRepository<T> Interface** (Repository.cs)
Defines the contract for all repository operations:
```csharp
public interface IRepository<T> where T: class
{
    T Get(int id);                                          // Get single entity by ID
    IEnumerable<T> GetAll();                               // Get all entities
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate); // Find by predicate
    
    void Add(T entity);                                    // Add single entity
    void AddRange(IEnumerable<T> entities);               // Add multiple entities
    
    void Update(T entity);                                // Update existing entity
    void Remove(T entity);                                // Remove single entity
    void RemoveRange(IEnumerable<T> entities);            // Remove multiple entities
}
```

### 2. **WorkingBookRepository<T>** (WorkingBookRepository.cs)
Implementation of IRepository using Entity Framework Core:
- Injects BookDbContext for database access
- Each operation includes SaveChanges() for persistence
- Uses generic constraints to work with any entity type

### 3. **BooksController** (BooksController.cs)
Full CRUD operations for Book management:

#### **Index Action**
- **Endpoint:** GET /Books
- **Purpose:** Display all books
- **Returns:** ViewResult with IEnumerable<Book>
- **Test:** `Index_Returns_All_Books`

#### **Details Action**
- **Endpoint:** GET /Books/Details/{id}
- **Purpose:** Display single book details
- **Returns:** ViewResult (book found) or NotFoundResult (404)
- **Tests:** 
  - `Details_Returns_Book_When_Exists`
  - `Details_Returns_NotFound_When_Book_Does_Not_Exist`

#### **Create GET Action**
- **Endpoint:** GET /Books/Create
- **Purpose:** Display create book form
- **Returns:** ViewResult with empty Book object
- **Test:** `Create_Get_Returns_View`

#### **Create POST Action**
- **Endpoint:** POST /Books/Create
- **Purpose:** Add new book to database
- **Returns:** RedirectToActionResult (success) or ViewResult (validation failure)
- **Test:** `Create_Post_Adds_Book_And_Redirects`

#### **Edit GET Action**
- **Endpoint:** GET /Books/Edit/{id}
- **Purpose:** Display edit book form
- **Returns:** ViewResult (book found) or NotFoundResult (404)
- **Tests:**
  - `Edit_Get_Returns_Book_When_Exists`
  - `Edit_Get_Returns_NotFound_When_Book_Does_Not_Exist`

#### **Edit POST Action**
- **Endpoint:** POST /Books/Edit/{id}
- **Purpose:** Update existing book
- **Returns:** RedirectToActionResult (success), BadRequestResult (ID mismatch), or ViewResult (validation)
- **Tests:**
  - `Edit_Post_Updates_Book_And_Redirects`
  - `Edit_Post_Returns_BadRequest_When_ID_Mismatch`

#### **Delete GET Action**
- **Endpoint:** GET /Books/Delete/{id}
- **Purpose:** Display delete confirmation
- **Returns:** ViewResult (book found) or NotFoundResult (404)
- **Tests:**
  - `Delete_Get_Returns_Book_When_Exists`
  - `Delete_Get_Returns_NotFound_When_Book_Does_Not_Exist`

#### **Delete POST Action (DeleteConfirmed)**
- **Endpoint:** POST /Books/Delete/{id}
- **Purpose:** Remove book from database
- **Returns:** RedirectToActionResult (success) or NotFoundResult (404)
- **Tests:**
  - `DeleteConfirmed_Removes_Book_And_Redirects`
  - `DeleteConfirmed_Returns_NotFound_When_Book_Does_Not_Exist`

### 4. **HomeController Extensions** (HomeController.cs)
Additional repository-based actions for the Home controller:

#### **Index Action** (Enhanced)
- Already uses repository.GetAll()
- Supports genre filtering via parameter
- Sets ViewData["Genre"]

#### **BooksByGenre Action**
- **Endpoint:** GET /Home/BooksByGenre?genre={genre}
- **Purpose:** Get books for specific genre using Find method
- **Returns:** ViewResult with filtered books or BadRequestObjectResult (400)
- **Tests:**
  - `BooksByGenre_Returns_Books_For_Fiction`
  - `BooksByGenre_Returns_BadRequest_When_Genre_Empty`
  - `BooksByGenre_Returns_BadRequest_When_Genre_Null`

#### **GenreCount Action**
- **Endpoint:** GET /Home/GenreCount?genre={genre}
- **Purpose:** Get count of books for specific genre
- **Returns:** ViewResult with ViewData["Count"] set
- **Tests:**
  - `GenreCount_Returns_Correct_Count`
  - `GenreCount_Returns_BadRequest_When_Genre_Empty`

#### **GetBook Action**
- **Endpoint:** GET /Home/GetBook/{id}
- **Purpose:** Get single book by ID using repository.Get()
- **Returns:** ViewResult (found) or NotFoundResult (404)
- **Tests:**
  - `GetBook_Returns_Book_When_Exists`
  - `GetBook_Returns_NotFound_When_Book_Does_Not_Exist`

## Testing Strategy

### **Test Framework:** Microsoft.VisualStudio.TestTools.UnitTesting
### **Mocking Framework:** Telerik JustMock

### **Test Categories:**

#### 1. **Positive Tests** (Happy Path)
- Verify successful operations
- Check correct return types
- Validate data integrity

#### 2. **Negative Tests** (Error Handling)
- Test missing resources (NotFound)
- Test invalid input (BadRequest)
- Test validation failures

#### 3. **Boundary Tests**
- Empty strings for genre
- Null values
- Non-existent IDs

#### 4. **Mock Setup Patterns**

**Simple Return Mock:**
```csharp
Mock.Arrange(() => bookRepository.GetAll())
    .Returns(books)
    .MustBeCalled();
```

**Parameter Matching Mock:**
```csharp
Mock.Arrange(() => bookRepository.Get(1))
    .Returns(book)
    .MustBeCalled();
```

**Action Mock (Void Methods):**
```csharp
Mock.Arrange(() => bookRepository.Add(book))
    .DoNothing()
    .MustBeCalled();
```

**Expression Predicate Mock:**
```csharp
Mock.Arrange(() => bookRepository.Find(
    Arg.IsAny<Expression<Func<Book, bool>>>()))
    .Returns(books)
    .MustBeCalled();
```

### **Test Coverage Summary**

| Component | Test Count | Coverage |
|-----------|-----------|----------|
| BooksController.Index | 1 | ✅ All scenarios |
| BooksController.Details | 2 | ✅ Found, Not Found |
| BooksController.Create GET | 1 | ✅ Form display |
| BooksController.Create POST | 1 | ✅ Save operation |
| BooksController.Edit GET | 2 | ✅ Found, Not Found |
| BooksController.Edit POST | 2 | ✅ Update, ID mismatch |
| BooksController.Delete GET | 2 | ✅ Found, Not Found |
| BooksController.Delete POST | 2 | ✅ Delete, Not Found |
| HomeController.BooksByGenre | 3 | ✅ Filter, validation |
| HomeController.GenreCount | 2 | ✅ Count, validation |
| HomeController.GetBook | 2 | ✅ Found, Not Found |
| **Total Tests** | **20+** | **✅ Comprehensive** |

## Dependency Injection Flow

```
Program.cs
  ├─ AddDbContext<BookDbContext>()
  │   └─ SQL Server configuration
  ├─ AddScoped<IRepository<Book>, WorkingBookRepository<Book>>()
  │   └─ Registers repository
  └─ AddControllersWithViews()
      ├─ BooksController
      │   └─ Constructor receives IRepository<Book>
      └─ HomeController
          └─ Constructor receives IRepository<Book>
```

## Repository Pattern Benefits

✅ **Abstraction:** Data access logic separated from business logic
✅ **Testability:** Easy to mock repository for unit testing
✅ **Reusability:** Same repository used across multiple controllers
✅ **Maintainability:** Change data source without affecting controllers
✅ **Flexibility:** Support multiple data sources (SQL, MongoDB, etc.)

## Running the Tests

1. Open Test Explorer in Visual Studio
2. Build the solution
3. Run all tests: `Test → Run All Tests`
4. Run specific test class: Right-click test class → Run Tests
5. Run specific test method: Right-click test method → Run Tests

## Assertions Used in Tests

- `Assert.AreEqual()` - Verify expected value matches actual
- `Assert.IsNotNull()` - Verify object exists
- `Assert.IsNull()` - Verify object is null
- `Assert.IsTrue()` - Verify condition is true
- `Assert.IsInstanceOfType()` - Verify type is correct

## Best Practices Implemented

✅ Each test follows AAA pattern (Arrange-Act-Assert)
✅ Descriptive test names indicate what is being tested
✅ Mock configuration is clear and explicit
✅ One assertion per logical concept
✅ Tests are independent and repeatable
✅ Both happy path and error paths tested
✅ Consistent naming conventions across tests
