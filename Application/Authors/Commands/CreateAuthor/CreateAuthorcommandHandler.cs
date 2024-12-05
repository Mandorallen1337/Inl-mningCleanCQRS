using Application.DTOs.AuthorDto;
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

namespace Application.Authors.Commands.CreateAuthor
{
    public class CreateAuthorcommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<Author>>
    {
        private readonly IGenericRepository<Author> _genericRepository;

        public CreateAuthorcommandHandler(IGenericRepository<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<Author>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CreateAuthorDto.FirstName) ||
                string.IsNullOrEmpty(request.CreateAuthorDto.LastName))
            {
                return OperationResult<Author>.FailureResult("First name and last name are required");
            }
            var newAuthor = new Author(request.CreateAuthorDto.FirstName, request.CreateAuthorDto.LastName);
            await _genericRepository.AddAsync(newAuthor);
            return OperationResult<Author>.SuccessResult(newAuthor);
        }
    }
}
