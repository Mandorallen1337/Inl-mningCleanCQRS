using Database.Databases;
using Domain.Models;
using Domain.Repositories;
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
        private readonly IGenericRepository<Author> _genericRepository;

        public GetAuthorByIdQueryHandler(IGenericRepository<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var foundAuthor = await _genericRepository.GetByIdAsync(request.AuthorId);
            if (foundAuthor == null)
            {
                throw new Exception($"Author not found {request.AuthorId}");
            }
            return foundAuthor;
        }
    }
}
