using Application.DTOs;
using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<Author>>
    {
        private readonly IGenericRepository<Author> _genericRepository;

        public UpdateAuthorCommandHandler(IGenericRepository<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<Author>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = await _genericRepository.GetByIdAsync(request.AuthorId);
            if (authorToUpdate == null)
            {
                return OperationResult<Author>.FailureResult("Author not found");

            }
            authorToUpdate.FirstName = request.UpdateAuthorDto.FirstName;
            authorToUpdate.LastName = request.UpdateAuthorDto.LastName;
            await _genericRepository.UpdateAsync(authorToUpdate);
            return OperationResult<Author>.SuccessResult(authorToUpdate);

        }
    }
}
