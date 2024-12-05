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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<BookController>
        [HttpGet("GetAllBooks")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            try
            {
                var operationResult = await _mediator.Send(new GetAllBooksQuery());
                if (operationResult.IsSuccess)
                {
                    return Ok(new { message = operationResult.Message, data = operationResult.Data });
                }
                else
                {
                    return BadRequest(new { message = operationResult.Message, operationResult.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<BookController>/5
        [HttpGet("GetBookById")]
        public async Task<ActionResult<Book>> GetBookById(Guid bookId)
        {
            try
            {
                var foundBook = await _mediator.Send(new GetBookbyIdQuery(bookId));
                if (foundBook.IsSuccess)
                {
                    return Ok(new { message = foundBook.Message, data = foundBook.Data });
                }
                return BadRequest(foundBook.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<BookController>
        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand createBookCommand)
        {
            try
            {
                var newbook = await _mediator.Send(createBookCommand);
                if(newbook.IsSuccess)
                {
                    return Ok(new { message = newbook.Message, data = newbook.Data });
                }
                return BadRequest(new { message = newbook.Message, newbook.ErrorMessage });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }            
        }

        // DELETE api/<BookController>/5
        [HttpDelete("DeleteBook")]
        public async Task<ActionResult<Book>> DeleteBook(Guid id)
        {
            try
            {                
                var booktodelete = await _mediator.Send(new DeleteBookCommand(id));

                if (booktodelete.IsSuccess)
                {
                    
                    return Ok(new { message = booktodelete.Message, data = booktodelete.Data });
                }
                
                return BadRequest(new { message = booktodelete.Message, booktodelete.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateBook")]
        public async Task<IActionResult> Updatebook(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {            
            try
            {
                var updateBook = await _mediator.Send(new UpdateBookCommand(id, updateBookDto));
                if (updateBook.IsSuccess)
                {
                    return Ok(new { message = updateBook.Message, data = updateBook.Data });
                }
                return BadRequest(new { message = updateBook.Message, updateBook.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }
    }
    
}