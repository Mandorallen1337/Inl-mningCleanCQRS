using Application.Authors.Commands.DeleteAuthor;
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
    public class DeleteAuthorCommandHandlerTests
    {
        private IGenericRepository<Author> _fakeRepository;
        private DeleteAuthorCommandHandler _deleteAuthorCommandHandler;

        [SetUp]
        public void SetUp()
        {
            _fakeRepository = A.Fake<IGenericRepository<Author>>();
            _deleteAuthorCommandHandler = new DeleteAuthorCommandHandler(_fakeRepository);
        }

        [Test]
        public async Task Handle_WhenAuthorIsDeleted_ReturnsSuccessResult()
        {
            // Arrange
            Guid authorId = Guid.NewGuid();
            var authorToDelete = new Author("John", "Doe");
            authorToDelete.GetType().GetProperty("Id").SetValue(authorToDelete, authorId);

            var deleteCommand = new DeleteAuthorCommand(authorId);

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns(authorToDelete);
            A.CallTo(() => _fakeRepository.DeleteAsync(authorToDelete)).Returns(Task.CompletedTask);

            // Act
            var result = await _deleteAuthorCommandHandler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Id, Is.EqualTo(authorId));

            A.CallTo(() => _fakeRepository.DeleteAsync(authorToDelete)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_WhenAuthorIsNotFound_ReturnsFailureResult()
        {
            // Arrange
            Guid authorId = Guid.NewGuid();
            var deleteCommand = new DeleteAuthorCommand(authorId);

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns((Author)null);

            // Act
            var result = await _deleteAuthorCommandHandler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));

            A.CallTo(() => _fakeRepository.DeleteAsync(A<Author>.Ignored)).MustNotHaveHappened();
        }
    }
}
