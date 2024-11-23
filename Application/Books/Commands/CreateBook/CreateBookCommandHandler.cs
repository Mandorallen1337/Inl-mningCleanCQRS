using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly FakeDatabase _fakeDatabase;

        public CreateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _fakeDatabase.Books.Add(request.NewBook);
            }
            catch
            {
                throw new Exception("Book not added");
            }            
            return Task.FromResult(request.NewBook);
        }
    }
}
