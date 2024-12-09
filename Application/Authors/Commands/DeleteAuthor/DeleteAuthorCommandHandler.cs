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
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<Author>>
    {
        private readonly IGenericRepository<Author> _genericRepository;

        public DeleteAuthorCommandHandler(IGenericRepository<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<Author>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            // Check if author exists
            Author authorToDelete = await _genericRepository.GetByIdAsync(request.AuthorId);
            if(authorToDelete == null)
            {
                return OperationResult<Author>.FailureResult("Author not found");
            }
            await _genericRepository.DeleteAsync(authorToDelete);
            return OperationResult<Author>.SuccessResult(authorToDelete);                     
        }
    }
}
