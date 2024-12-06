using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Security
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<object> _passwordhasher;
        private readonly object _dummyInstance = new();

        public PasswordService(IPasswordHasher<object> passwordhasher)
        {
            _passwordhasher = passwordhasher;
        }

        public string HashPassword(string password)
        {
            return _passwordhasher.HashPassword(_dummyInstance, password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var result = _passwordhasher.VerifyHashedPassword(_dummyInstance, passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
