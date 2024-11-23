using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAllauthors;
using Application.DTOs.AuthorDto;
using Database.Databases;
using Domain.Models;
using NUnit.Framework;

namespace BookTests
{
    public class Tests    
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task When_GetAllAuthors_Then_AllAuthorsAreReturned()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new GetAllAuthorsQueryHandler(fakeDatabase);
            // Act

            var authors = await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(authors);
        }

        [Test]
        public async Task When_GetAllAuthorsById_Then_AuthorIsReturned()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new GetAllAuthorsQueryHandler(fakeDatabase);
            var author = new Author("Test", "johnnysAuthor");
            fakeDatabase.Authors.Add(author);
            // Act

            var authors = await handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(authors);
            Assert.IsTrue(authors.Any(x => x.Id == author.Id));
        }

        [Test]
        public async Task When_CreateAuthorCommand_IsHandled_Then_AuthorIsAddedToList()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new CreateAuthorcommandHandler(fakeDatabase);
            var initialAuthorCount = fakeDatabase.Authors.Count;
            var newAuthor = new Author("Test", "johnnysAuthor");

            // Act
            await handler.Handle(new CreateAuthorCommand(newAuthor), CancellationToken.None);

            // Assert
            Assert.AreEqual(initialAuthorCount + 1, fakeDatabase.Authors.Count, "The authors list should now contain 1 more author than before.");
            Assert.IsTrue(fakeDatabase.Authors.Contains(newAuthor), "The new author should be in the authors list.");
        }

        [Test]
        public async Task When_DeleteAuthorCommand_IsHandled_Then_AuthorIsRemovedFromList()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new DeleteAuthorCommandHandler(fakeDatabase);
            var initialAuthorCount = fakeDatabase.Authors.Count;
            var authorToDelete = fakeDatabase.Authors.First();

            // Act
            await handler.Handle(new DeleteAuthorCommand(authorToDelete.Id), CancellationToken.None);

            // Assert
            Assert.AreEqual(initialAuthorCount - 1, fakeDatabase.Authors.Count, "The authors list should now contain 1 less author than before.");
            Assert.IsFalse(fakeDatabase.Authors.Contains(authorToDelete), "The author to delete should no longer be in the authors list.");
        }

        [Test]
        public async Task When_UpdateAuthorCommand_IsHandled_Then_AuthorIsUpdated()
        {
            // Arrange
            var fakeDatabase = new FakeDatabase();
            var handler = new UpdateAuthorCommandHandler(fakeDatabase);
            var authorToUpdate = fakeDatabase.Authors.First();

            var updateAuthorDto = new UpdateAuthorDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName"
            };

            // Act
            await handler.Handle(new UpdateAuthorCommand(authorToUpdate.Id, updateAuthorDto), CancellationToken.None);

            // Assert
            Assert.AreEqual(updateAuthorDto.FirstName, authorToUpdate.FirstName);
            Assert.AreEqual(updateAuthorDto.LastName, authorToUpdate.LastName);
        }

    }
}
