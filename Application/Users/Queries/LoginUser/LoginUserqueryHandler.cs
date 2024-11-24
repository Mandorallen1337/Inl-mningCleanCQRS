using Application.Users.Queries.LoginUser.Helpers;
using Database.Databases;
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
        private readonly FakeDatabase _fakeDatabase;
        private readonly TokenHelper _tokenHelper;
        public LoginUserqueryHandler(FakeDatabase fakeDatabase, TokenHelper tokenHelper)
        {
            _fakeDatabase = fakeDatabase;
            _tokenHelper = tokenHelper;
        }

        public Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _fakeDatabase.Users.FirstOrDefault(u => u.UserName == request.LoginUserDto.UserName && u.Password == request.LoginUserDto.Password);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid username or password");
                }
                
                string token = _tokenHelper.GenerateJwtToken(user);


                return Task.FromResult(token);
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
