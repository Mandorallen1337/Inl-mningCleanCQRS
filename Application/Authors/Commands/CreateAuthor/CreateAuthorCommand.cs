using Application.DTOs.AuthorDto;
using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<OperationResult<Author>>
    {
        public CreateAuthorDto CreateAuthorDto;

        public CreateAuthorCommand(CreateAuthorDto createAuthorDto)
        {
            CreateAuthorDto = createAuthorDto;            
        }

        public Author NewAuthor { get; }


    }
}
