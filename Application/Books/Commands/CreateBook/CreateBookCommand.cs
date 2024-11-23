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
    public class CreateBookCommand : IRequest<Book>
    {
        
        public CreateBookCommand(Book bookToAdd)
        {
            NewBook = bookToAdd;
        }

        public Book NewBook { get; set; }
    }
}
