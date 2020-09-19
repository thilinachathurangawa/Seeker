using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Seeker.Constants;
using Seeker.Models;
using Seeker.ViewModels;

namespace Seeker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInMAnager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
		{
			_userManager = userManager;
			_signInMAnager = signInManager;
            _appSettings = appSettings.Value;
        }

		[HttpPost]
		[Route("Register")]
		//post ApplicationUser/Register
		public async Task<JsonResult> PostApplicationUser(ApplicaitonUserViewModel model)
		{
			try
			{
				var applicationUser = new ApplicationUser();


				applicationUser.FirstName = model.FirstName;
				applicationUser.Email = model.Email;
				applicationUser.LastName = model.LastName;
				applicationUser.UserName = model.Email;
				if (model.IsProvider)
				{
					applicationUser.UserType = UserType.ServiceProvider;
				}
				else
				{
					applicationUser.UserType = UserType.Client;
				}

				{
					var result = await _userManager.CreateAsync(applicationUser, model.Password);
					if (result == IdentityResult.Success)
					{
						return new JsonResult(new { Status = ResponseCodes.LoginSuccess });
					}
					else
					{
						return new JsonResult(new { Status = ResponseCodes.LoginFailed });
					}
					// return Ok(result);
				}
			}
			catch (Exception)
			{
				return new JsonResult(new { Status = ResponseCodes.LoginFailed });
			}
		}

		[HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(); 
        }

    }
}