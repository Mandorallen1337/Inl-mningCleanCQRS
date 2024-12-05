using Database.Databases;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IGenericRepository<User> _genericRepository;

        public GetAllUsersQueryHandler(IGenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await _genericRepository.GetAllAsync();
            if (allUsers == null)
            {
                return OperationResult<List<User>>.FailureResult("No users found");
            }
            return OperationResult<List<User>>.SuccessResult(allUsers.ToList());
        }
    }
}
