using Application;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using Application.DTOs.BookDto;
using Database.Databases;
using Domain.Models;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task When_CreateBookCommand_IsHandled_Then_BookIsAddedToList()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new CreateBookCommandHandler(fakeDatabase);
            var initialBookCount = fakeDatabase.Books.Count;
            var newBook = new Book("Test", "johnnysBook");


            // Act
            await handler.Handle(new CreateBookCommand(newBook), CancellationToken.None);

            // Assert
            Assert.AreEqual(initialBookCount + 1, fakeDatabase.Books.Count, "The books list should now contain 1 more book than before.");
            Assert.IsTrue(fakeDatabase.Books.Contains(newBook), "The new book should be in the books list.");
        }

        [Test]
        public async Task When_DeleteBookCommand_IsHandled_Then_BookIsRemovedFromList()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new DeleteBookCommandHandler(fakeDatabase);
            var initialBookCount = fakeDatabase.Books.Count;
            var bookToDelete = fakeDatabase.Books.First();



            // Act
            await handler.Handle(new DeleteBookCommand(bookToDelete.Id), CancellationToken.None);

            // Assert
            Assert.AreEqual(initialBookCount - 1, fakeDatabase.Books.Count, "The books list should now contain 1 less book than before.");
            Assert.IsFalse(fakeDatabase.Books.Contains(bookToDelete), "The book to delete should no longer be in the books list.");
        }

        [Test]
        public async Task When_GetAllBooksQuery_IsHandled_Then_AllBooksAreReturned()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new GetAllBooksQueryHandler(fakeDatabase);

            // Act
            var books = await handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

            // Assert
            Assert.That(books.Count, Is.EqualTo(5), "Should return all 5 books from the fake database.");            
            Assert.That(books != null, "Should return all 5 books from the fake database.");

        }

        [Test]
        public async Task When_GetBookByIdQuery_IsHandled_Then_BookIsReturned()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new GetBookbyIdQueryHandler(fakeDatabase);
            var bookToGet = fakeDatabase.Books.First();

            // Act
            var book = await handler.Handle(new GetBookbyIdQuery(bookToGet.Id), CancellationToken.None);

            // Assert
            Assert.That(book != null, "Should return the book with the specified ID.");
            Assert.That(book.Id, Is.EqualTo(bookToGet.Id), "Should return the book with the specified ID.");
        }

        [Test]
        public async Task When_UpdateBookCommand_IsHandled_Then_BookIsUpdated()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new UpdateBookCommandHandler(fakeDatabase);
            var bookToUpdate = fakeDatabase.Books.First();
            var updateBookDto = new UpdateBookDto
            {
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act
            await handler.Handle(new UpdateBookCommand(bookToUpdate.Id, updateBookDto), CancellationToken.None);

            // Assert
            Assert.That(bookToUpdate.Title, Is.EqualTo(updateBookDto.Title), "The book's title should be updated.");
            Assert.That(bookToUpdate.Description, Is.EqualTo(updateBookDto.Description), "The book's description should be updated.");
        }
    }
}