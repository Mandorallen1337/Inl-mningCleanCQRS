using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {        
        private readonly IGenericRepository<Book> _genericRepository;

        public UpdateBookCommandHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booktoUpdate = await _genericRepository.GetByIdAsync(request.BookId) ?? throw new Exception($"Book not found {request.BookId}");
                booktoUpdate.Title = request.UpdateBookDto.Title;
                booktoUpdate.Description = request.UpdateBookDto.Description;

                await _genericRepository.UpdateAsync(booktoUpdate);
                return booktoUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating book: {ex.Message}", ex);
            }

        }
    }
}
