using Application.DTOs.UserDto;
using Domain.Models;
using MediatR;


namespace Application.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest <OperationResult<string>>
    {
        public LoginUserQuery(UserDto loginUserDto)
        {
            LoginUserDto = loginUserDto;
        }

        public UserDto LoginUserDto { get;}
    }
}
