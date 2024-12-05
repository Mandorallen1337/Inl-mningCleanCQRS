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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        //[Authorize]
        [HttpGet("GetAllAuthors")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var getAllUsers = await _mediator.Send(new GetAllAuthorsQuery());
                if (getAllUsers.IsSuccess)
                {
                    return Ok(new { message = getAllUsers.Message, data = getAllUsers.Data });
                }
                else
                {
                    return BadRequest(new { message = getAllUsers.Message, getAllUsers.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(Guid authorId)
        {
            try
            {
                var getUserById = await _mediator.Send(new GetAuthorByIdQuery(authorId));
                if (getUserById.IsSuccess)
                {
                    return Ok(new { message = getUserById.Message, data = getUserById.Data });
                }
                else
                {
                    return BadRequest(new { message = getUserById.Message, getUserById.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST api/<AuthorController>
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            try
            {
                if (createAuthorDto == null)
                {
                    return BadRequest("Author data is null.");
                }
                var createAuthor = await _mediator.Send(new CreateAuthorCommand(createAuthorDto));
                if (createAuthor.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetAuthorById), new { authorId = createAuthor.Data.Id }, createAuthor.Data);
                }
                else
                {
                    return BadRequest(new { message = createAuthor.Message, createAuthor.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var deleteAuthor = await _mediator.Send(new DeleteAuthorCommand(id));
                if (deleteAuthor.IsSuccess)
                {
                    return Ok(new { message = deleteAuthor.Message });
                }
                else
                {
                    return BadRequest(new { message = deleteAuthor.Message, deleteAuthor.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto updateAuthorDto)
        {
            try
            {
                var updateAuthor = await _mediator.Send(new UpdateAuthorCommand(id, updateAuthorDto));
                if (updateAuthor.IsSuccess)
                {
                    return Ok(new { message = updateAuthor.Message });
                }
                else
                {
                    return BadRequest(new { message = updateAuthor.Message, updateAuthor.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            // Log the exception (not implemented here)
            return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
        }
    }
}
