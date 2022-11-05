﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IUserRepository
    {
        User GetById(Guid? id);
        User GetByLogin(string? login);
        void Add(User user);
        void Update(User user);
        User CheckUserLogin(User user);
        User CheckUserEmail(User user);
        User CheckUserExistence(string login, string password);
    }
}
