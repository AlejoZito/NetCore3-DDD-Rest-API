using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Infrastructure;
using NetCore3_api.WebApi.DTOs;
using NetCore3_api.WebApi.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace NetCore3_api.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IRepository<User> _userRepository;
        readonly AppDbContext _dbContext;
        public UserController(
            IRepository<User> userRepository,
            AppDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }


        /// <summary>
        /// Create user. This endpoint has no validations (just for creating test users)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("users/")]
        [Produces(typeof(Charge))]
        public async Task<ActionResult> Create(
            [FromBody]string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                if (await _userRepository.FindAsync(x => x.Username == username) != null)
                    return BadRequest("Username is taken");

                var user = _userRepository.Insert(new Domain.Models.Aggregates.User.User() { Username = username });
                await _dbContext.SaveChangesAsync();
                return Ok(user);
            }
            else
                return BadRequest("Invalid username");
        }
    }
}