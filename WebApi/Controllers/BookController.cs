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
                var books = await _mediator.Send(new GetAllBooksQuery());
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<BookController>/5
        [HttpGet("GetBookById")]
        public async Task<ActionResult<Book>> GetBookById(Guid bookId)
        {
            try
            {
                var book = await _mediator.Send(new GetBookbyIdQuery(bookId));
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
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
            if (createBookCommand == null)
            {
                return BadRequest("Book data is null.");
            }

            try
            {
                var result = await _mediator.Send(createBookCommand);
                return Ok(result);
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("DeleteBook")]
        public async Task<ActionResult<Book>> DeleteBook(Guid id)
        {
            try
            {                
                var result = await _mediator.Send(new DeleteBookCommand(id));

                if (result == null)
                {
                    
                    return NotFound();
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateBook")]
        public async Task<IActionResult> Updatebook(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            if (updateBookDto == null)
            {
                return BadRequest("Book data is null.");
            }

            try
            {
                var result = await _mediator.Send(new UpdateBookCommand(id, updateBookDto));
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            
        }
    }
    
}