using Application.DTOs.UserDto;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.LoginUser;
using Database.Databases;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        internal readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _mediator.Send(new GetAllUsersQuery()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto NewUser)
        {
            try
            {
                return Ok(await _mediator.Send(new AddNewUserCommand(NewUser)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto userWantingtoLogin)
        {
            try
            {
                return Ok(await _mediator.Send(new LoginUserQuery(userWantingtoLogin)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
