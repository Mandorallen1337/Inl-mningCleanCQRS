using Application.Authors.Commands.UpdateAuthor;
using Application.DTOs.AuthorDto;
using Domain.Models;
using Domain.Repositories;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTests.FakeItEasy
{
    [TestFixture]
    public class UpdateAuthorCommandHandlerTests
    {
        private IGenericRepository<Author> _fakeRepository;
        private UpdateAuthorCommandHandler _updateAuthorCommandHandler;

        [SetUp]
        public void SetUp()
        {
            _fakeRepository = A.Fake<IGenericRepository<Author>>();
            _updateAuthorCommandHandler = new UpdateAuthorCommandHandler(_fakeRepository);
        }

        [Test]
        public async Task Handle_WhenAuthorIsUpdated_ReturnsSuccessResult()
        {
            // Arrange
            Guid authorId = Guid.NewGuid();
            var authorToUpdate = new UpdateAuthorDto
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var updateCommand = new UpdateAuthorCommand(authorId, authorToUpdate);
            var existingAuthor = new Author("Jane", "Doe");
            existingAuthor.GetType().GetProperty("Id").SetValue(existingAuthor, authorId);

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns(existingAuthor);
            A.CallTo(() => _fakeRepository.UpdateAsync(A<Author>.Ignored)).Returns(Task.CompletedTask);
            // Act
            var result = await _updateAuthorCommandHandler.Handle(updateCommand, CancellationToken.None);


            // Assert
            Assert.That(result.IsSuccess, Is.True);

            A.CallTo(() => _fakeRepository.UpdateAsync(A<Author>.That.Matches(author =>
            author.Id == authorId &&
            author.FirstName == authorToUpdate.FirstName &&
            author.LastName == authorToUpdate.LastName
            ))).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task Handle_WhenAuthorIsNotFound_ReturnsFailureResult()
        {
            // Arrange
            Guid authorId = Guid.NewGuid();
            var authorToUpdate = new UpdateAuthorDto
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var updateCommand = new UpdateAuthorCommand(authorId, authorToUpdate);

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns((Author)null);

            // Act
            var result = await _updateAuthorCommandHandler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));
        }
    }
}
