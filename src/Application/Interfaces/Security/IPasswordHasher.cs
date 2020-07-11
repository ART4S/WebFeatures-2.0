﻿namespace Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string ComputeHash(string password);

        bool Verify(string hash, string expectedPassword);
    }
}
