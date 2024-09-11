using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Infrastructure
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
