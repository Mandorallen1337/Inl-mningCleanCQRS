using Database.Databases;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAllUsersQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<User> allUsersFormFakeDatabase = _fakeDatabase.Users;
                return Task.FromResult(allUsersFormFakeDatabase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
