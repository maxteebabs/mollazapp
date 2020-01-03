using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Context;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly MollazDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;

        public DefaultUserService(
            MollazDbContext context
            , IMapper mapper, UserManager<UserEntity> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<User> GetUserAsync(Guid userId)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null) return null;
            return _mapper.Map<User>(entity);
//            var resource = new User()
//            {
//                Href = null,//Url.Link(nameof(getUserById), new {userId = entity.Id}),
//                FirstName = entity.FirstName,
//                LastName = entity.LastName,
//                Email = entity.Email,
//                Phone = entity.Phone,
//                DateCreated = entity.DateCreated,
//                Location = new Address(){ City = entity.City, Country = entity.Country},
//            };
//            return resource;
        }

        public async Task<PagedResults<User>> GetUsersAsync(PagingOptions pagingOptions)
        {
//            var query = _context.Users.ProjectTo<User>(_mapper.ConfigurationProvider);
//            return await query.ToArrayAsync();
            var query = _context.Users.Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>(_mapper.ConfigurationProvider);
//            return await query.ToArrayAsync();
            return new PagedResults<User>()
            {
                Items = query.ToArray(),
                TotalSize = await _context.Users.CountAsync()
            };
        }
        public async Task<PagedResults<User>> GetOrderedUsersAsync(PagingOptions pagingOptions
            , SortOptions<User, UserEntity> sortOptions)
        {
            IQueryable<UserEntity> query = _context.Users;
            query = sortOptions.Apply(query);
            var items = query.Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>(_mapper.ConfigurationProvider);
//            return await query.ToArrayAsync();
            return new PagedResults<User>()
            {
                Items = items.ToArray(),
                TotalSize = await query.CountAsync()
            };
        }

        public async Task<PagedResults<User>> SearchUsersAsync(PagingOptions pagingOptions, SortOptions<User
            , UserEntity> sortOptions, SearchOptions<User, UserEntity> searchOptions)
        {
            IQueryable<UserEntity> query = _context.Users;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);
            var items = query.Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>(_mapper.ConfigurationProvider);
//            return await query.ToArrayAsync();
            return new PagedResults<User>()
            {
                Items = items.ToArray(),
                TotalSize = await query.CountAsync()
            };
        }
        public async Task<Guid> CreateUserAsync(UserForm userForm)
        {
            var userId = Guid.NewGuid();
            var entity = new UserEntity()
            {
                Id = userId,
                FirstName= userForm.FirstName,
                LastName = userForm.LastName,
                Email = userForm.Email,
                Phone= userForm.Phone,
                DateCreated = DateTime.UtcNow,
                City = userForm.City,
                Country = userForm.Country,
                Password = userForm.Password
            };
//            _context.Users.Add(entity);
//            var created = await _context.SaveChangesAsync();
//            if(created < 1) throw new InvalidOperationException("Could not Create the booking");
//            return userId;
            var result = await _userManager.CreateAsync(entity, userForm.Password);
            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description;
                return userId;
            }

            return userId;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(p => p.Id == userId);
            if(user == null) throw new ArgumentException("Invalid User Id");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> UpdateUserAsync(Guid userId, UserForm userForm)
        {
            var user = await _context.Users.SingleOrDefaultAsync(p => p.Id == userId);
            if(user == null) throw new ArgumentException("Invalid User Id");
            user.FirstName = userForm.FirstName;
            user.LastName = userForm.LastName;
            user.Phone = userForm.Phone;
            _context.Users.Update(user);
            var updated = await _context.SaveChangesAsync();
            if(updated < 1) throw new InvalidOperationException("Could not update the user");
            return userId;
        }
    }
}