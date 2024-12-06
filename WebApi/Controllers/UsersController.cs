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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetAllUsers")]
        [ResponseCache(CacheProfileName = "DefaultCache")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Processing request to get all users.");

            try
            {
                var getAllUsers = await _mediator.Send(new GetAllUsersQuery());

                if (getAllUsers.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved all users.");
                    return Ok(new { message = getAllUsers.Message, data = getAllUsers.Data });
                }

                _logger.LogWarning("Failed to retrieve all users. Reason: {ErrorMessage}", getAllUsers.ErrorMessage);
                return BadRequest(new { message = getAllUsers.Message, error = getAllUsers.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                return HandleException(ex);
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto newUser)
        {
            _logger.LogInformation("Processing request to register a new user.");
            try
            {                
                if (!ModelState.IsValid)
                {                    
                    _logger.LogWarning("RegisterUser: The provided user data is invalid.");
                    return BadRequest(ModelState);
                }

                var registerUser = await _mediator.Send(new AddNewUserCommand(newUser));

                if (registerUser.IsSuccess)
                {
                    _logger.LogInformation("Successfully registered a new user with ID: {UserId}", registerUser.Data?.Id);
                    return CreatedAtAction(nameof(GetAllUsers), new { id = registerUser.Data?.Id }, registerUser.Data);
                }

                _logger.LogWarning("Failed to register a new user. Reason: {ErrorMessage}", registerUser.ErrorMessage);
                return BadRequest(new { message = registerUser.Message, error = registerUser.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a new user.");
                return HandleException(ex);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto userWantingToLogin)
        {
            _logger.LogInformation("Processing request to log in a user.");
            try
            {              

                if (userWantingToLogin == null)
                {
                    _logger.LogWarning("LoginUser: The provided user data is null.");
                    return BadRequest(new { message = "Login data cannot be null." });
                }                

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("LoginUser: The provided user data is invalid.");
                    return BadRequest(ModelState);
                }

                var loginUser = await _mediator.Send(new LoginUserQuery(userWantingToLogin));

                if (loginUser.IsSuccess)
                {
                    _logger.LogInformation("Successfully logged in user: {UserName}", userWantingToLogin.UserName);
                    return Ok(new { message = loginUser.Message, data = loginUser.Data });
                }

                _logger.LogWarning("Failed login attempt for user: {UserName}. Reason: {ErrorMessage}",
                    userWantingToLogin.UserName, loginUser.ErrorMessage);

                return Unauthorized(new { message = loginUser.Message, error = loginUser.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {       
        return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }
}
