using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookbyIdQueryHandler : IRequestHandler<GetBookbyIdQuery, Book>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetBookbyIdQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<Book> Handle(GetBookbyIdQuery request, CancellationToken cancellationToken)
        {
            var book = _fakeDatabase.Books.FirstOrDefault(x => x.Id == request.BookId);
            if (book == null)
            {
                throw new Exception($"Book not found {request.BookId}");
            }
            return Task.FromResult(book);
        }
    }
}
