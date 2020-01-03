using System;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Context;
using WebApplication.Models;

namespace WebApplication.Seeds
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            await AddTestUser(services.GetRequiredService<RoleManager<UserRoleEntity>>(),
                services.GetRequiredService<UserManager<UserEntity>>());
            await AddTestData(services.GetRequiredService<MollazDbContext>());
        }
        public static async Task AddTestData(MollazDbContext context)
        {
//            if (context.Users.Any())
//            {
//                //Already has Data
//                return;
//            }

            context.Users.Add(new UserEntity()
            {
                Id = Guid.NewGuid(),
                FirstName= "Makinde",
                LastName = "Ola",
                Email = "marks@yahoo.com",
                Phone= "2348152425698",
                DateCreated = DateTime.Now,
                City = "Lekki",
                Country = "Nigeria",
                UserName = "Makinde",
                Password = "@@Password123",
            });
            
            context.Users.Add(new UserEntity()
            {
                Id = Guid.NewGuid(),
                FirstName= "Stella",
                LastName = "Damascus",
                Email = "stella@yahoo.com",
                Phone= "2348152425699",
                DateCreated = DateTime.Now,
                City = "Chicago",
                Country = "United States Of America",
                UserName = "Stella",
                Password = "@@Password123",
            });
            context.Users.Add(new UserEntity()
            {
                Id = Guid.NewGuid(),
                FirstName= "Ezekiel",
                LastName = "Famurewa",
                Email = "famurewa_taiwo@yahoo.com",
                Phone= "2348152425698",
                DateCreated = DateTime.Now,
                City = "Manchester",
                Country = "London",
                UserName = "Ezekiel",
                Password = "@@Password123",
            });
            var result = await context.SaveChangesAsync();
        }

        public static async Task AddTestUser(RoleManager<UserRoleEntity> roleManager
            , UserManager<UserEntity> userManager)
        {
            var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
            if (dataExists)
            {
                return;
            }
            //add a test role
            var roleResult = await roleManager.CreateAsync(new UserRoleEntity("Admin"));
            //add test user
            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                FirstName= "Admin",
                LastName = "Admin",
                Email = "admin@yahoo.com",
                Phone= "2348152425698",
                DateCreated = DateTime.Now,
                City = "Birmingham",
                Country = "London"
            };
            user.UserName = "admin";
            var userResult = await userManager.CreateAsync(user, "@@Password123");
            //put user in the admin role
            var result = await userManager.AddToRoleAsync(user, "Admin");
            result = await userManager.UpdateAsync(user);
        }
    }
}