using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAllauthors;
using Application.Authors.Queries.GetAuthorById;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetBookById;
using Application.DTOs.AuthorDto;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<AuthorController>
        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _mediator.Send(new GetAllAuthorsQuery()));
        }

        [HttpGet("GetAuthorById")]
        public async Task <IActionResult> GetAuthorById(Guid authorId)
        {
            return Ok(await _mediator.Send(new GetAuthorByIdQuery(authorId)));
            
        }

        // POST api/<AuthorController>
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            if (createAuthorDto == null)
            {
                return BadRequest();
            }
            var authorToAdd = new Author(createAuthorDto.FirstName, createAuthorDto.LastName);
            var createdAuthor = await _mediator.Send(new CreateAuthorCommand(authorToAdd));
            return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
        }

        

        // DELETE api/<AuthorController>/5
        [HttpDelete("DeleteAuthorById")]
        public async Task<IActionResult> DeleteAuthorById(Guid id)
        {
            return Ok(await _mediator.Send(new DeleteAuthorCommand(id)));
        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto updateAuthorDto)
        {
            return Ok(await _mediator.Send(new UpdateAuthorCommand(id, updateAuthorDto)));
        }
    }
}
