using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid Id);
        Task<PagedResults<User>> GetUsersAsync(PagingOptions pagingOptions);
        Task<PagedResults<User>> GetOrderedUsersAsync(PagingOptions pagingOptions
            , SortOptions<User, UserEntity> sortOptions);

        Task<PagedResults<User>> SearchUsersAsync(PagingOptions pagingOptions, SortOptions<User
            , UserEntity> sortOptions, SearchOptions<User, UserEntity> searchOptions);

        Task<Guid> UpdateUserAsync(Guid userId, UserForm userForm);
        Task<Guid> CreateUserAsync(UserForm userForm);
        Task DeleteUserAsync(Guid userId);
    }
}