using Domain.Models;
using Domain.Repositories;
using MediatR;


namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<Book>>>
    {
        private readonly IGenericRepository<Book> _genericRepository;

        public GetAllBooksQueryHandler(IGenericRepository<Book> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<OperationResult<List<Book>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            
             var books = await _genericRepository.GetAllAsync();
             if (books == null || !books.Any())
             {
                return OperationResult<List<Book>>.FailureResult("No books found");
            }

             return OperationResult<List<Book>>.SuccessResult(books.ToList());


        }
    }
}
