using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Book>
    {
        private readonly FakeDatabase _fakeDatabase;

        public DeleteBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<Book> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookToDelete = _fakeDatabase.Books.FirstOrDefault(x => x.Id == request.BookId);
            if (bookToDelete == null)
            {
                throw new Exception($"Book not found {request.BookId}");
            }           
            
            _fakeDatabase.Books.Remove(bookToDelete);
            
            return Task.FromResult(bookToDelete);
        }
    }
}
