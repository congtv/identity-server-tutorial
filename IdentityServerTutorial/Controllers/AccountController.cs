using IdentityServerTutorial.Common;
using IdentityServerTutorial.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ResultModel> Register([FromBody] RegistUserRequestModel requestModel)
        {
            if(!ModelState.IsValid)
            {
                return new ResultModel
                {
                    Message = "Invalid data",
                    Status = Status.Error,
                    Data = string.Join("", ModelState.Keys.Select(e => "<li>" + e + "</li>"))
                };
            }

            var user = await userManager.FindByNameAsync(requestModel.UserName);
            if(user != null)
            {
                return new ResultModel
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>User already exists</li>"
                };
            }

            user = new ApplicationUser
            {
                ID = Guid.NewGuid().ToString(),
                UserName = requestModel.UserName,
                Email = requestModel.Email
            };

            var result = await userManager.CreateAsync(user, requestModel.Password);

            if (result.Succeeded)
            {
                return new ResultModel
                {
                    Status = Status.Success,
                    Message = "User Created",
                    Data = user
                };
            }
            else
            {
                var resultErrors = result.Errors.Select(e => "<li>" + e.Description + "</li>");
                return new ResultModel
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = string.Join("", resultErrors)
                };
            }
        }

        [HttpPost]
        public async Task<ResultModel> Login([FromBody] LoginUserRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return new ResultModel
                {
                    Message = "Invalid data",
                    Status = Status.Error,
                    Data = string.Join("", ModelState.Keys.Select(e => "<li>" + e + "</li>"))
                };
            }

            var user = await userManager.FindByNameAsync(requestModel.UserName);
            var checkPw = await userManager.CheckPasswordAsync(user, requestModel.Password);
            if (user != null && checkPw)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ID));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return new ResultModel
                {
                    Status = Status.Success,
                    Message = "Succesfull login",
                    Data = requestModel
                };
            }
            else
            {
                return new ResultModel
                {
                    Status = Status.Error,
                    Message = "Invalid data",
                    Data = "<li>Invalid Username or Password</li>"
                };
            }
        }

        [HttpGet]
        [Authorize]
        public UserClaims Claims()
        {
            var claims = User.Claims.Select(c => new ClaimResultModel
            {
                Type = c.Type,
                Value = c.Value
            });

            return new UserClaims
            {
                UserName = User.Identity.Name,
                Claims = claims
            };
        }

        [HttpGet]
        public UserStateModel Authenticated()
        {
            return new UserStateModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Username = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty
            };
        }

        [HttpPost]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
