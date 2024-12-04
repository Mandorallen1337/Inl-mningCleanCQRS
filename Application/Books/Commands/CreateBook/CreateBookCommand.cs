using Application.DTOs.BookDto;
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
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }

        public CreateBookCommand(string title, string description, Guid authorId)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
        }
    }
}
