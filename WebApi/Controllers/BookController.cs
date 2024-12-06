using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using Application.DTOs.BookDto;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;
        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<BookController>
        [HttpGet("GetAllBooks")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation("Start processing GetAllBooks request.");
            try
            {
                var getAllBooks = await _mediator.Send(new GetAllBooksQuery());
                if (getAllBooks.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved books. Count: {BookCount}", getAllBooks.Data.Count);
                    return Ok(new { message = getAllBooks.Message, data = getAllBooks.Data });
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve books. Reason: {ErrorMessage}", getAllBooks.ErrorMessage);
                    return BadRequest(new { message = getAllBooks.Message, error = getAllBooks.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while retrieving books.");
                return HandleException(ex);
            }
        }

        // GET api/<BookController>/5
        [HttpGet("GetBookById")]
        public async Task<IActionResult> GetBookById(Guid bookId)
        {
            _logger.LogInformation("Processing request to retrieve book with ID: {BookId}", bookId);
            try
            {
                var foundBook = await _mediator.Send(new GetBookbyIdQuery(bookId));

                if (foundBook.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved book with ID: {BookId}", bookId);
                    return Ok(new { message = foundBook.Message, data = foundBook.Data });
                }

                _logger.LogWarning("Failed to retrieve book with ID: {BookId}. Reason: {ErrorMessage}", bookId, foundBook.ErrorMessage);
                return BadRequest(new { message = foundBook.Message, error = foundBook.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving book with ID: {BookId}", bookId);
                return HandleException(ex);
            }
        }


        // POST api/<BookController>
        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand createBookCommand)
        {
            _logger.LogInformation("Processing request to create a new book.");
            try
            {                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("CreateBook: The provided book data is invalid.");
                    return BadRequest(ModelState);  
                }

                var newBook = await _mediator.Send(createBookCommand);

                if (newBook.IsSuccess)
                {
                    _logger.LogInformation("Successfully created a new book with title: {BookTitle}", createBookCommand.Title);
                    return CreatedAtAction(nameof(GetBookById), new { id = newBook.Data?.Id }, newBook.Data);  
                }

                _logger.LogWarning("Failed to create a new book. Reason: {ErrorMessage}", newBook.ErrorMessage);
                return BadRequest(new { message = newBook.Message, error = newBook.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new book.");
                return HandleException(ex);  
            }
        }


        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            _logger.LogInformation("Processing request to delete a book with ID: {BookId}", id);
            try
            {
                var bookToDelete = await _mediator.Send(new DeleteBookCommand(id));

                if (bookToDelete.IsSuccess)
                {
                    _logger.LogInformation("Successfully deleted book with ID: {BookId}", id);
                    return Ok(new { message = bookToDelete.Message, data = bookToDelete.Data });
                }

                _logger.LogWarning("Failed to delete book with ID: {BookId}. Reason: {ErrorMessage}", id, bookToDelete.ErrorMessage);
                return BadRequest(new { message = bookToDelete.Message, error = bookToDelete.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting book with ID: {BookId}", id);
                return HandleException(ex);
            }
        }

        [HttpPut("UpdateBook")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            _logger.LogInformation("Processing request to update book with ID: {BookId}", id);
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("UpdateBook: The provided book data is invalid.");
                    return BadRequest(ModelState);
                }

                var updateBook = await _mediator.Send(new UpdateBookCommand(id, updateBookDto));

                if (updateBook.IsSuccess)
                {
                    _logger.LogInformation("Successfully updated book with ID: {BookId}", id);
                    return Ok(new { message = updateBook.Message, data = updateBook.Data });
                }

                _logger.LogWarning("Failed to update book with ID: {BookId}. Reason: {ErrorMessage}", id, updateBook.ErrorMessage);
                return BadRequest(new { message = updateBook.Message, error = updateBook.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating book with ID: {BookId}", id);
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
        }
    }

}