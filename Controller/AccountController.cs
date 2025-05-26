using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
           try {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

            if(createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
                if(roleResult.Succeeded)
                {
                    return Ok("User created successfully");
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            } else {
                return StatusCode(500, createdUser.Errors);
            }

           } catch (Exception ex) {
               return StatusCode(500, ex);
           }
        }
    }
}