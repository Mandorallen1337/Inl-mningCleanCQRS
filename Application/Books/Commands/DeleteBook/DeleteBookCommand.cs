using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<Book>
    {
        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; }
    }
}
