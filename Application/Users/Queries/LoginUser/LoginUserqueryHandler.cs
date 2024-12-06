using Application.Users.Queries.LoginUser.Helpers;
using Database.Databases;
using Database.Security;
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
    public class LoginUserqueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly IPasswordService _passwordService;

        public LoginUserqueryHandler(IGenericRepository<User> userRepository, TokenHelper tokenHelper, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _passwordService = passwordService;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            
            var user = await _userRepository.FindByAsync(u => u.UserName == request.LoginUserDto.UserName);
            
            if (user == null)
            {
                return OperationResult<string>.FailureResult("Invalid username or password");
            }

            // Verify the provided password against the stored hash
            if (!_passwordService.VerifyPassword(request.LoginUserDto.Password, user.Password))
            {
                return OperationResult<string>.FailureResult("Invalid username or password");
            }

            // Generate JWT token if password verification succeeds
            string token = _tokenHelper.GenerateJwtToken(user);
            return OperationResult<string>.SuccessResult(token);
        }


    }
}
