using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication.Context;
using WebApplication.Infrastructure;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        [HttpGet("GetCustomer", Name = nameof(GetCustomer))]
        public IActionResult GetCustomer()
        {
            var response = "Hello World";
//            _logger.Log(LogLevel.Debug());
            return Ok(response);
        }
        [HttpGet(Name = nameof(Customers))]
        [ProducesResponseType(200)]
        public IActionResult Customers()
        {
            throw new NotImplementedException("Not implemented");
        }

        private readonly User _user;
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;
        public UserController(IOptions<User> userWrapper
            , IUserService userService
            , IOptions<PagingOptions> defaultPagingOptions)
        {
            _user = userWrapper.Value;
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }
        [HttpGet("getUser", Name = nameof(GetUser))]
        [ProducesResponseType(200)]
        public ActionResult<User> GetUser()
        {
            _user.Href = Url.Link(nameof(GetUser), null);
            return _user;
        }
        
        [HttpGet("getUsers", Name = nameof(GetUsers))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ResponseCache(CacheProfileName = "Static")]
        [ETag]
//        [ResponseCache(Duration = 86400, VaryByQueryKeys = new []{"offset", "limit","orderBy","search"})]
        public async Task<ActionResult<Models.Collection<User>>> GetUsers(
            [FromQuery] PagingOptions pagingOptions = null)
        {
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            var data = await _userService.GetUsersAsync(pagingOptions);
            var currentRouteName = nameof(GetUsers);
            var collection = PagedCollection<User>.Create(
                Link.ToCollection(currentRouteName),
                data.Items.ToArray(),
                data.TotalSize,
                pagingOptions
            );
//            var collection = new PagedCollection<User>
//            {
//                Self = Link.To(nameof(GetUsers)),
//                Value = data.Items.ToArray(),
//                Size = data.TotalSize,
//                Offset = pagingOptions.Offset.Value,
//                Limit = pagingOptions.Limit.Value
////                data = data.To,
////                Self = Link.To(nameof(GetUsers))
//            };
            return collection;
        }
        
        [HttpGet("getOrderedUsers", Name = nameof(GetOrderedUsers))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Collection<User>>> GetOrderedUsers(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions)
        {
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            var data = await _userService.GetOrderedUsersAsync(pagingOptions, sortOptions);
            var currentRouteName = nameof(GetOrderedUsers);
            var collection = PagedCollection<User>.Create(
                Link.ToCollection(currentRouteName),
                data.Items.ToArray(),
                data.TotalSize,
                pagingOptions
            );
            return collection;
        }
        
        [HttpGet("searchUsers", Name = nameof(SearchUsers))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Models.Collection<User>>> SearchUsers(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions)
        {
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            var data = await _userService.SearchUsersAsync(pagingOptions, sortOptions, searchOptions);
            var currentRouteName = nameof(SearchUsers);
            var collection = PagedCollection<User>.Create(
                Link.ToCollection(currentRouteName),
                data.Items.ToArray(),
                data.TotalSize,
                pagingOptions
            );
            return collection;
        }
        
        [HttpGet("{UserId}", Name = nameof(GetUserById))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<User>> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost("Create", Name = nameof(Create))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult> Create([FromBody] UserForm userForm)
        {
            var userId = await _userService.CreateUserAsync(userForm);
            return Created(
                Url.Link(nameof(GetUserById),
                new {userId}),
            null);
//            return new ObjectResult();
        }
        [HttpPut("{userId}", Name = nameof(Update))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Update(Guid userId, [FromBody] UserForm userForm)
        {
            var user = _userService.GetUserAsync(userId);
            if (user == null) return NotFound();
            var result = await _userService.UpdateUserAsync(userId, userForm);
            return Created(
                Url.Link(nameof(GetUserById),
                    new {userId}),
                null);
        }
        
        [HttpDelete("{userId}", Name = nameof(Delete))]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = _userService.GetUserAsync(userId);
            if (user == null) return NotFound();
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}