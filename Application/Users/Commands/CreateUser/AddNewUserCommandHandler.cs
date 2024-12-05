using Database.Databases;
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
    internal class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IGenericRepository<User> _genericRepository;

        public AddNewUserCommandHandler(IGenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {        
            if (string.IsNullOrWhiteSpace(request.UserDto.UserName) || string.IsNullOrWhiteSpace(request.UserDto.Password))
            {
                return OperationResult<User>.FailureResult("Username and password are required");
            }
             User userToCreate = new User
             {
                 Id = Guid.NewGuid(),
                 UserName = request.UserDto.UserName,
                 Password = request.UserDto.Password
             };
             await _genericRepository.AddAsync(userToCreate);
             return OperationResult<User>.SuccessResult(userToCreate);                     
            
        }
            
        
    }
}
