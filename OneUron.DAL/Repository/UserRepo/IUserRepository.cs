﻿using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserRepo
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUserNameAndPasswordAsync(string userName, string password);
        Task<User> GetByIdWithRolesAsync(Guid userId);
        Task<User> GetByUserNameWithRolesAsync(string userName);
        Task AssignRoleToUserAsync(Guid userId, string roleName);
    }
}
