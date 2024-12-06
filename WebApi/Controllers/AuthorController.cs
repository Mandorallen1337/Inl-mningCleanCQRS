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
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<AuthorController>
        //[Authorize]
        [HttpGet("GetAllAuthors")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllAuthors()
        {
            _logger.LogInformation("Getting all authors from the database");
            try
            {
                var getAllUsers = await _mediator.Send(new GetAllAuthorsQuery());
                if (getAllUsers.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved all authors from the database. Count: {AuthorCount}", getAllUsers.Data.Count);
                    return Ok(new { message = getAllUsers.Message, data = getAllUsers.Data });
                }
                else
                {
                    _logger.LogError("Failed to retrieve all authors from the database. Reason: {Errormessege}", getAllUsers.ErrorMessage);
                    return BadRequest(new { message = getAllUsers.Message, getAllUsers.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occured while retriving authors.");
                return HandleException(ex);
            }
        }

        [HttpGet("GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(Guid authorId)
        {
            _logger.LogInformation("Getting author with {AuthorId} from the database", authorId);
            try
            {
                var getUserById = await _mediator.Send(new GetAuthorByIdQuery(authorId));
                if (getUserById.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved author with ID: {AuthorId}", authorId);
                    return Ok(new { message = getUserById.Message, data = getUserById.Data });
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve book with ID: {AuthorId}. Reason: {ErrorMessage}", authorId, getUserById.ErrorMessage);
                    return BadRequest(new { message = getUserById.Message, getUserById.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving book with ID: {AuthorId}", authorId);
                return HandleException(ex);
            }
        }

        // POST api/<AuthorController>
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            _logger.LogInformation("Processing request to create a new author.");

            try
            {
                if (createAuthorDto == null)
                {
                    _logger.LogWarning("CreateAuthorDto is null.");
                    return BadRequest(new { message = "Author data cannot be null." });
                }

                var createAuthor = await _mediator.Send(new CreateAuthorCommand(createAuthorDto));

                if (createAuthor.IsSuccess)
                {
                    _logger.LogInformation("Successfully created a new author with ID: {AuthorId}", createAuthor.Data.Id);
                    return CreatedAtAction(nameof(GetAuthorById), new { authorId = createAuthor.Data.Id }, createAuthor.Data);
                }

                _logger.LogWarning("Failed to create a new author. Reason: {ErrorMessage}", createAuthor.ErrorMessage);
                return BadRequest(new { message = createAuthor.Message, error = createAuthor.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new author.");
                return HandleException(ex);
            }
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            _logger.LogInformation("Processing request to delete author with ID: {AuthorId}", id);

            try
            {
                var deleteAuthor = await _mediator.Send(new DeleteAuthorCommand(id));

                if (deleteAuthor.IsSuccess)
                {
                    _logger.LogInformation("Successfully deleted author with ID: {AuthorId}", id);
                    return Ok(new { message = deleteAuthor.Message });
                }

                _logger.LogWarning("Failed to delete author with ID: {AuthorId}. Reason: {ErrorMessage}", id, deleteAuthor.ErrorMessage);
                return BadRequest(new { message = deleteAuthor.Message, error = deleteAuthor.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting author with ID: {AuthorId}", id);
                return HandleException(ex);
            }
        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto updateAuthorDto)
        {
            _logger.LogInformation("Processing request to update author with ID: {AuthorId}", id);

            try
            {
                var updateAuthor = await _mediator.Send(new UpdateAuthorCommand(id, updateAuthorDto));

                if (updateAuthor.IsSuccess)
                {
                    _logger.LogInformation("Successfully updated author with ID: {AuthorId}", id);
                    return Ok(new { message = updateAuthor.Message });
                }

                _logger.LogWarning("Failed to update author with ID: {AuthorId}. Reason: {ErrorMessage}", id, updateAuthor.ErrorMessage);
                return BadRequest(new { message = updateAuthor.Message, error = updateAuthor.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating author with ID: {AuthorId}", id);
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { error = ex.Message });
        }
    }
}
