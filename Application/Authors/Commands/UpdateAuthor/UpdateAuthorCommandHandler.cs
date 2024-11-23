using Application.DTOs;
using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = _fakeDatabase.Authors.FirstOrDefault(x => x.Id == request.AuthorId);
            if (authorToUpdate == null)
            {
                throw new Exception("Author not found");
            }
            else
            {
                authorToUpdate.FirstName = request.UpdateAuthorDto.FirstName;
                authorToUpdate.LastName = request.UpdateAuthorDto.LastName;
                return Task.FromResult(authorToUpdate);
            }
        }
    }
}
