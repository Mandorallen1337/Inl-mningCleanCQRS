using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookbyIdQueryHandler : IRequestHandler<GetBookbyIdQuery, Book>
    {
        private readonly IGenericRepository<Book> _genericRepository;

        public GetBookbyIdQueryHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<Book> Handle(GetBookbyIdQuery request, CancellationToken cancellationToken)
        {            
             var book = await _genericRepository.GetByIdAsync(request.BookId);
             return book ?? throw new KeyNotFoundException($"Book not found {request.BookId}");                      
        }
    }
}
