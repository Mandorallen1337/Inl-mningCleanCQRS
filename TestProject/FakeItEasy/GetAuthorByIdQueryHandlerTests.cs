using Application.Authors.Queries.GetAuthorById;
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
    public class GetAuthorByIdQueryHandlerTests
    {
        private IGenericRepository<Author> _fakeRepository; // Dependency for the repository
        private GetAuthorByIdQueryHandler _getAuthorByIdQueryHandler; // Class under test

        [SetUp] 
        public void SetUp()
        {
            _fakeRepository = A.Fake<IGenericRepository<Author>>(); // Create a fake instance of the repository
            _getAuthorByIdQueryHandler = new GetAuthorByIdQueryHandler(_fakeRepository); // Create an instance of the class under test with the fake repository
        }

        [Test] 
        public async Task Handle_WhenAuthorExists_ReturnsAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid(); // Generate a new unique author ID
            var author = new Author(firstName: "Stephen", lastName: "King"); // Create a new author object

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns(author); // Set up the fake repository to return the author when GetByIdAsync is called with the specified author ID

            // Act
            var result = await _getAuthorByIdQueryHandler.Handle(new GetAuthorByIdQuery(authorId), CancellationToken.None); // Call the Handle method of the class under test with a GetAuthorByIdQuery object containing the author ID

            // Assert
            Assert.That(result.Data, Is.EqualTo(author)); // Assert that the returned result contains the expected author object
        }

        [Test] // Indicates that this method is a test
        public async Task Handle_WhenAuthorDoesNotExist_ReturnsFailureResult()
        {
            // Arrange
            var authorId = Guid.NewGuid(); // Generate a new unique author ID

            A.CallTo(() => _fakeRepository.GetByIdAsync(authorId)).Returns((Author)null); // Set up the fake repository to return null when GetByIdAsync is called with the specified author ID

            // Act
            var result = await _getAuthorByIdQueryHandler.Handle(new GetAuthorByIdQuery(authorId), CancellationToken.None); // Call the Handle method of the class under test with a GetAuthorByIdQuery object containing the author ID

            // Assert
            Assert.That(result.IsSuccess, Is.False); // Assert that the returned result indicates failure
        }
    }
}
