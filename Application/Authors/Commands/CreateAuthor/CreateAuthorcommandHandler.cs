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
    public class CreateAuthorcommandHandler : IRequestHandler<CreateAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public CreateAuthorcommandHandler(FakeDatabase database)
        {
            _fakeDatabase = database;
        }

        public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _fakeDatabase.Authors.Add(request.NewAuthor);
                return await Task.FromResult(request.NewAuthor);
            }
            catch
            {
                throw new Exception("Author not added");
            }
        }
    }
}
