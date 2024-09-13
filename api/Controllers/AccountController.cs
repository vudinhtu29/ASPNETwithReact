using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,ITokenService tokenService,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if(user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded) return Unauthorized("Not found password");
            return Ok(
                new NewUserDto{
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
         } 
         

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser{
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var createUser = await _userManager.CreateAsync(appUser,registerDto.Password);
                if(createUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(appUser,"User");
                    if(roleResult.Succeeded){
                        return Ok(
                            new NewUserDto{
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else{
                        return StatusCode(500,roleResult.Errors);
                    }
                }
                else{
                    return StatusCode(500,createUser.Errors);
                }
            }catch(Exception e){
                return StatusCode(500,e);
            }
        }

        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var findAcc = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Username.ToLower());
            if(findAcc == null){
                return null;
            }
         
            return StatusCode(500,"Processing...");
        }
    }
}