using Application.DTOs.UserDto;
using MediatR;


namespace Application.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<string>
    {
        public LoginUserQuery(UserDto loginUserDto)
        {
            LoginUserDto = loginUserDto;
        }

        public UserDto LoginUserDto { get;}
    }
}
