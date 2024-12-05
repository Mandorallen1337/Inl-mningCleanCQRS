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
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<Book>>
    {
        private readonly IGenericRepository<Book> _bookRepository;

        private readonly IGenericRepository<Author> _authorRepository;

        public CreateBookCommandHandler(IGenericRepository<Book> writeRepository, IGenericRepository<Author> authorRepositary)
        {
            _bookRepository = writeRepository;
            _authorRepository = authorRepositary;
        }

        public async Task <OperationResult<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            {
                return OperationResult<Book>.FailureResult("Title and Description is requierd");
            }            

            var author = await _authorRepository.FindByAsync(x => x.Id == request.AuthorId);
            if (author == null)
            {
                return OperationResult<Book>.FailureResult("Author not found");
            }
            try
            {
                var book = new Book(request.Title, request.Description, request.AuthorId);
                await _bookRepository.AddAsync(book);
                return OperationResult<Book>.SuccessResult(book);
            }
            catch (Exception)
            {
                // Log error here
                return OperationResult<Book>.FailureResult("Error while creating book");
            }
            
        }
    }
}
