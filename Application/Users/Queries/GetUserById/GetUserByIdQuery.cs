using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<OperationResult<User>>
    {
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; set; }


    }
}
