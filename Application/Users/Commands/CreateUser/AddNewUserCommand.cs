using Application.DTOs.UserDto;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands.CreateUser
{
    public class AddNewUserCommand : IRequest<User>
    {
        public AddNewUserCommand(UserDto userDto)
        {
            UserDto = userDto;
        }
        public UserDto UserDto { get; set;}
    }
}
