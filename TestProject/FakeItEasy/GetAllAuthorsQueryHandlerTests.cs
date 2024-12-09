using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Authors.Queries.GetAllauthors;
using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace Tests.FakeItEasy
{
    [TestFixture]
    public class GetAllAuthorsQueryHandlerTests : IDisposable
    {

        private IGenericRepository<Author> _fakeRepository;
        private GetAllAuthorsQueryHandler _handler;
        private IMemoryCache _fakeMemoryCache;
                

        [SetUp]
        public void SetUp()
        {
            _fakeRepository = A.Fake<IGenericRepository<Author>>();
            _fakeMemoryCache = A.Fake<IMemoryCache>();

            var authors = new List<Author>
            {
            new Author("Stephen", "King"),
            new Author("Camilla", "Läckberg")
            };

            A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(authors.AsQueryable());

            // Simulate cache miss
            object cacheValue;
            A.CallTo(() => _fakeMemoryCache.TryGetValue(A<object>.Ignored, out cacheValue)).Returns(false);

            // Simulate cache entry creation
            var cacheEntry = A.Fake<ICacheEntry>();
            A.CallTo(() => _fakeMemoryCache.CreateEntry(A<object>.Ignored)).Returns(cacheEntry);

            _handler = new GetAllAuthorsQueryHandler(_fakeRepository, _fakeMemoryCache);
        }

        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        [Test]
        public async Task Handle_ShouldReturnAllAuthors()
        {
            // Act
            var result = await _handler.Handle(new GetAllAuthorsQuery(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Data.Count, Is.EqualTo(2));
            Assert.That(result.Data[0].FirstName, Is.EqualTo("Stephen"));
            Assert.That(result.Data[0].LastName, Is.EqualTo("King"));
            Assert.That(result.Data[1].FirstName, Is.EqualTo("Camilla"));
            Assert.That(result.Data[1].LastName, Is.EqualTo("Läckberg"));

            A.CallTo(() => _fakeRepository.GetAllAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeMemoryCache.CreateEntry(A<object>.Ignored)).MustHaveHappenedOnceExactly();
        }

        public void Dispose()
        {
            // Ensure proper cleanup of disposable resources
            (_fakeMemoryCache as IDisposable)?.Dispose();
            _fakeMemoryCache = null;
            _fakeRepository = null;
            _handler = null;
        }
    }
}
