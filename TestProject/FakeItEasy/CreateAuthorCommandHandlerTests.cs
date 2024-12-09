using Application.Authors.Commands.CreateAuthor;
using Application.DTOs.AuthorDto;
using Domain.Models;
using Domain.Repositories;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BookTests.FakeItEasy
{
    [TestFixture]
    public class CreateAuthorCommandHandlerTests
    {
        private IGenericRepository<Author> _fakeRepository;
        private CreateAuthorcommandHandler _createAuthorCommandHandler;

        [SetUp]
        public void SetUp()
        {
            _fakeRepository = A.Fake<IGenericRepository<Author>>();
            _createAuthorCommandHandler = new CreateAuthorcommandHandler(_fakeRepository);
        }

        [Test]
        public async Task Handle_WhenAuthorIsCreated_ReturnsSuccessResult()
        {
            var authorDto = new CreateAuthorDto
            {
                FirstName = "Stephen",
                LastName = "King"
            };

            var createCommand = new CreateAuthorCommand(authorDto);

            // Mocka att AddAsync körs utan fel
            A.CallTo(() => _fakeRepository.AddAsync(A<Author>.That.Matches(a =>
                a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName)))
                .DoesNothing();

            // Act
            var result = await _createAuthorCommandHandler.Handle(createCommand, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.FirstName, Is.EqualTo("Stephen"));
            Assert.That(result.Data.LastName, Is.EqualTo("King"));

            // Verifiera att AddAsync anropades
            A.CallTo(() => _fakeRepository.AddAsync(A<Author>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_WhenDtoIsInvalid_ReturnsFailureResult()
        {
            // Arrange
            var invalidDto = new CreateAuthorDto
            {
                FirstName = "",
                LastName = "King"
            };

            var createCommand = new CreateAuthorCommand(invalidDto);

            // Act
            var result = await _createAuthorCommandHandler.Handle(createCommand, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("First name and last name are required"));

            // Verifiera att AddAsync aldrig anropas
            A.CallTo(() => _fakeRepository.AddAsync(A<Author>.Ignored)).MustNotHaveHappened();
        }

    }
}
