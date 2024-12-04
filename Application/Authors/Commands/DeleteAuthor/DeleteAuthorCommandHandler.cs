using Database.Databases;
using Database.Exceptions;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Author>
    {
        private readonly IGenericRepository<Author> _genericRepository;

        public DeleteAuthorCommandHandler(IGenericRepository<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Author> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            Author authorToDelete = await _genericRepository.GetByIdAsync(request.AuthorId) ?? throw new NotFoundException($"Author not found {request.AuthorId}");
            await _genericRepository.DeleteAsync(authorToDelete);
            return authorToDelete;                      
        }
    }
}
