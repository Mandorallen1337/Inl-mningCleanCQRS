using Application.Users.Queries.LoginUser.Helpers;
using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.LoginUser
{
    public class LoginUserqueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public LoginUserqueryHandler(IGenericRepository<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByAsync(u => u.UserName == request.LoginUserDto.UserName && u.Password == request.LoginUserDto.Password) ?? throw new UnauthorizedAccessException("Invalid username or password");
            string token = _tokenHelper.GenerateJwtToken(user);
            return token;
        }
    
    }
}
