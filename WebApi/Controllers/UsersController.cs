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
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var getAllUsers = await _mediator.Send(new GetAllUsersQuery());
                if (getAllUsers.IsSuccess)
                {
                    return Ok(new { message = getAllUsers.Message, data = getAllUsers.Data });
                }
                return BadRequest(new { message = getAllUsers.Message, getAllUsers.ErrorMessage });                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto newUser)
        {
            try
            {
                var user = await _mediator.Send(new AddNewUserCommand(newUser));

                if (user.IsSuccess)
                {
                    return Ok(new { message = user.Message, data = user.Data });
                }
                return BadRequest(new { message = user.Message, user.ErrorMessage });
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
                var user = await _mediator.Send(new LoginUserQuery(userWantingtoLogin));
                if (user.IsSuccess)
                {
                    return Ok(new { message = user.Message, data = user.Data });
                }
                return BadRequest(new { message = user.Message, user.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
