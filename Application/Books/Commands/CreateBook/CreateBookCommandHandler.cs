using Application.DTOs.BookDto;
using Database.Databases;
using Database.Exceptions;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly IGenericRepository<Book> _bookRepository;

        private readonly IGenericRepository<Author> _authorRepository;

        public CreateBookCommandHandler(IGenericRepository<Book> writeRepository, IGenericRepository<Author> authorRepositary)
        {
            _bookRepository = writeRepository;
            _authorRepository = authorRepositary;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Title))
            {
                throw new ValidationException("Title is required");
            }
            if (string.IsNullOrEmpty(request.Description))
            {
                throw new ValidationException("Description is required");
            }

            var author = await _authorRepository.FindByAsync(x => x.Id == request.AuthorId) ?? throw new NotFoundException("Author not found");
            var book = new Book(request.Title, request.Description, request.AuthorId);
            await _bookRepository.AddAsync(book);
            return book;
        }
    }
}
