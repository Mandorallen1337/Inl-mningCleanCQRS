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
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<Book>>
    {
        private readonly IGenericRepository<Book> _genericRepository;

        public DeleteBookCommandHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }


        public async Task<OperationResult<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToDelete = await _genericRepository.GetByIdAsync(request.BookId);
                if (bookToDelete == null)
                {
                    return OperationResult<Book>.FailureResult("Book not found");
                }

                await _genericRepository.DeleteAsync(bookToDelete);
                
                return OperationResult<Book>.SuccessResult(bookToDelete);
            }
            catch(Exception ex)
            {
                // Log error
                return OperationResult<Book>.FailureResult("Error while deleting book");
            }
        }
    }
}
