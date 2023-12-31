﻿using CourseManagementProject.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Infrastructure.Implementation
{
    public class PasswordHandler : IPasswordHandler
    {
        public void CreateSaltAndHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return AreEqualArrays(computedHash, passwordHash);
        }

        private static bool AreEqualArrays(byte[] computed, byte[] passed)
        {
            for (var i = 0; i < computed.Length; i++)
            {
                if (computed[i] != passed[i])
                    return false;
            }

            return true;
        }
    }
}
