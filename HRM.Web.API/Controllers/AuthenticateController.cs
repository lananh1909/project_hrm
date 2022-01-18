using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HRM.Web.Core;
using HRM.Web.Core.Entities;
using HRM.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using HRM.Web.Core.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HRM.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        #region Field
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserInfoService _userInfoService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion

        #region Contructor
        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserInfoService userInfoService,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userInfoService = userInfoService;
            _signInManager = signInManager;
        }
        #endregion

        #region Method
        [HttpPost]
        [Route("register")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(register.Email);
                var traceId = Guid.NewGuid().ToString();
                if (user != null)
                {
                    return BadRequest(new ErrorServiceResult()
                    {
                        DevMessage = ErrorConstants.EmailExistMessage,
                        UserMessage = ErrorConstants.EmailExistMessage,
                        ErrorCode = ErrorConstants.EmailExist,
                        TraceId = traceId,
                        MoreInfo = ""
                    });
                }
                var emailRegex = new Regex(Constants.EmailRegex);
                var matchEmail = emailRegex.Match(register.Email);
                if (!matchEmail.Success)
                {
                    return BadRequest(new ErrorServiceResult()
                    {
                        DevMessage = ErrorConstants.InvalidEmailMessage,
                        UserMessage = ErrorConstants.InvalidEmailMessage,
                        ErrorCode = ErrorConstants.InvalidEmail,
                        TraceId = traceId,
                        MoreInfo = ""
                    });
                }
                var newUser = new ApplicationUser()
                {
                    Email = register.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = register.Email
                };
                newUser.CreatedDate = DateTime.Now;
                newUser.ModifiedDate = DateTime.Now;
                var result = await _userManager.CreateAsync(newUser, register.Password);
                if (result.Succeeded)
                {
                    _userInfoService.Add(new UserInfo() { FirstName = register.FirstName, LastName = register.LastName, UserId = newUser.Id, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now });
                    return Ok(new ServiceResult()
                    {
                        Success = true,
                        Data = { }
                    });
                }
                else
                {
                    var builder = new StringBuilder();
                    foreach (var item in result.Errors)
                    {
                        builder.Append(item.Description);
                    }
                    return StatusCode(400, new ErrorServiceResult()
                    {
                        DevMessage = builder.ToString(),
                        UserMessage = ErrorConstants.RegisterFailMessage,
                        ErrorCode = ErrorConstants.RegisterFail,
                        TraceId = traceId,
                        MoreInfo = ""
                    });
                }
            } catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (checkPassword)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    return Ok(new ServiceResult()
                    {
                        Success = true,
                        Data = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var traceId = Guid.NewGuid().ToString();
            if (user != null)
            {
                return BadRequest(new ErrorServiceResult()
                {
                    DevMessage = ErrorConstants.EmailExistMessage,
                    UserMessage = ErrorConstants.EmailExistMessage,
                    ErrorCode = ErrorConstants.EmailExist,
                    TraceId = traceId,
                    MoreInfo = ""
                });
            }
            var emailRegex = new Regex(Constants.EmailRegex);
            var matchEmail = emailRegex.Match(model.Email);
            if (!matchEmail.Success)
            {
                return BadRequest(new ErrorServiceResult()
                {
                    DevMessage = ErrorConstants.InvalidEmailMessage,
                    UserMessage = ErrorConstants.InvalidEmailMessage,
                    ErrorCode = ErrorConstants.InvalidEmail,
                    TraceId = traceId,
                    MoreInfo = ""
                });
            }
            var newUser = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };
            newUser.CreatedDate = DateTime.Now;
            newUser.ModifiedDate = DateTime.Now;
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                _userInfoService.Add(new UserInfo() { FirstName = model.FirstName, LastName = model.LastName, UserId = newUser.Id, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now });
                if(! await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if(! await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
                if(await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                }
                return Ok(new ServiceResult()
                {
                    Success = true,
                    Data = { }
                });
            }
            else
            {
                var builder = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    builder.Append(item.Description);
                }
                return StatusCode(400, new ErrorServiceResult()
                {
                    DevMessage = builder.ToString(),
                    UserMessage = ErrorConstants.RegisterFailMessage,
                    ErrorCode = ErrorConstants.RegisterFail,
                    TraceId = traceId,
                    MoreInfo = ""
                });
            }
        }
        [HttpGet]
        [Route("google")]
        public async Task<IActionResult> LoginGoogle([FromQuery] string returnUrl)
        {
            var redirectUrl = Url.Action("GoogleCallback", new { returnUrl = returnUrl});

            //var properties = new AuthenticationProperties { RedirectUri = "https://localhost:44384/signin-google" };
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("google/callback")]
        public async Task<IActionResult> GoogleCallback(string returnUrl)
        {
            var userInfor = await _signInManager.GetExternalLoginInfoAsync();
            ApplicationUser user = null;
            if(userInfor == null)
            {
                return BadRequest();
            } else
            {
                user = await _userManager.FindByLoginAsync(userInfor.LoginProvider, userInfor.Principal.FindFirstValue(ClaimTypes.NameIdentifier));
                if(user == null)
                {
                    var lastName = userInfor.Principal.FindFirstValue(ClaimTypes.GivenName);
                    var firstName = userInfor.Principal.FindFirstValue(ClaimTypes.Surname);
                    var fullName = userInfor.Principal.FindFirstValue(ClaimTypes.Name);
                    var email = userInfor.Principal.FindFirstValue(ClaimTypes.Email);
                    user = await _userManager.FindByEmailAsync(email);
                    if(user != null)
                    {
                        _userManager.AddLoginAsync(user, userInfor);
                    } else
                    {
                        user = new ApplicationUser()
                        {
                            UserName = email,
                            Email = email,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        var res = await _userManager.CreateAsync(user);
                        if (res.Succeeded)
                        {
                            _userInfoService.Add(new UserInfo()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                UserId = user.Id,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            });
                            await _userManager.AddLoginAsync(user, userInfor);
                        } else
                        {
                            return BadRequest();
                        }
                    }
                    
                }
            }
            var token = await GenerateTokenAsync(user);
            return Redirect(returnUrl + "?token=" + token);
        }
        #endregion

        #region Private Method
        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
