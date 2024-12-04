using Database.Databases;
using Domain.Models;
using Domain.Repositories;
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
        private readonly IGenericRepository<Book> _genericRepository;

        public DeleteBookCommandHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }


        public async Task<Book> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToDelete = await _genericRepository.GetByIdAsync(request.BookId) ?? throw new Exception($"Book not found {request.BookId}");
                await _genericRepository.DeleteAsync(bookToDelete);
                
                return bookToDelete;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting book: {ex.Message}", ex);
            }
        }
    }
}
