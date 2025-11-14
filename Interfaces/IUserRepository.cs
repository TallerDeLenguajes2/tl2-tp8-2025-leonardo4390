using System;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}