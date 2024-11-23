using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookbyIdQuery : IRequest<Book>
    {
        public GetBookbyIdQuery(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; }
    }
    
    
}
