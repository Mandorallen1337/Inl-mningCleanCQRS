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

namespace Application.Users.Commands.CreateUser
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IPasswordService _passwordService;

        public AddNewUserCommandHandler(IGenericRepository<User> genericRepository, IPasswordService passwordService)
        {
            _genericRepository = genericRepository;
            _passwordService = passwordService;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {        
            if (string.IsNullOrWhiteSpace(request.UserDto.UserName) || string.IsNullOrWhiteSpace(request.UserDto.Password))
            {
                return OperationResult<User>.FailureResult("Username and password are required");
            }

            string passwordHash = _passwordService.HashPassword(request.UserDto.Password);

            User userToCreate = new User
             {
                 Id = Guid.NewGuid(),
                 UserName = request.UserDto.UserName,
                 Password = passwordHash
             };
             await _genericRepository.AddAsync(userToCreate);
             return OperationResult<User>.SuccessResult(userToCreate);                     
            
        }
            
        
    }
}
