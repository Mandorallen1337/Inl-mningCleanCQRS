using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAuthorByIdQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var foundAuthor = _fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.AuthorId);
            if (foundAuthor == null)
            {
                throw new Exception($"Author not found {request.AuthorId}");
            }
            return Task.FromResult(foundAuthor);
        }
    }
}
