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
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return Ok(await _mediator.Send(new GetAllBooksQuery()));
        }

        // GET api/<BookController>/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Book>> GetBookById(Guid bookId)
        {
            return Ok(await _mediator.Send(new GetBookbyIdQuery(bookId)));
        }

        // POST api/<BookController>
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            if (createBookDto == null)
            {
                return BadRequest();
            }
            var bookToadd = new Book(createBookDto.Title, createBookDto.Description);
            var createdBook = await _mediator.Send(new CreateBookCommand(bookToadd));
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<List<Book>>> DeleteBook(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteBookCommand(id)));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Updatebook(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            return Ok(await _mediator.Send(new UpdateBookCommand(id, updateBookDto)));
        }
    }
}