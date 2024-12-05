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
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult< Book>>
    {        
        private readonly IGenericRepository<Book> _genericRepository;

        public UpdateBookCommandHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<Book>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booktoUpdate = await _genericRepository.GetByIdAsync(request.BookId);
                if (booktoUpdate == null)
                {
                    return OperationResult<Book>.FailureResult("Book not found");
                }
                booktoUpdate.Title = request.UpdateBookDto.Title;
                booktoUpdate.Description = request.UpdateBookDto.Description;

                await _genericRepository.UpdateAsync(booktoUpdate);
                return OperationResult<Book>.SuccessResult(booktoUpdate);
            }
            catch (Exception ex)
            {
                // Log error
                return OperationResult<Book>.FailureResult("Error while updating book");
            }

        }
    }
}
