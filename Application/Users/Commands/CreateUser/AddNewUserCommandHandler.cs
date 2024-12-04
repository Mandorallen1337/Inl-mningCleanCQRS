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
    internal class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, User>
    {
        private readonly IGenericRepository<User> _genericRepository;

        public AddNewUserCommandHandler(IGenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<User> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {        
            if (string.IsNullOrWhiteSpace(request.UserDto.UserName) || string.IsNullOrWhiteSpace(request.UserDto.Password))
            {
                throw new ArgumentException("User's username and password are required.");
            }
             User userToCreate = new User
             {
                 Id = Guid.NewGuid(),
                 UserName = request.UserDto.UserName,
                 Password = request.UserDto.Password
             };
             await _genericRepository.AddAsync(userToCreate);
             return userToCreate;                      
            
        }
            
        
    }
}
