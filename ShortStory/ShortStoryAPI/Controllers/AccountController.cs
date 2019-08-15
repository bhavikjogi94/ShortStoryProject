using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShortStoryAPI.ViewModel;
using ShortStoryBOL;

namespace ShortStoryAPI.Controllers
{

    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UserManager<SSUser> userManager;
        SignInManager<SSUser> signInManager;

        public AccountController(SignInManager<SSUser> _signInManager, UserManager<SSUser> _userManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new SSUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Date = model.DOB
                    };

                    var userResult = await userManager.CreateAsync(user, model.Password);
                    if (userResult.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, "User");
                        if (roleResult.Succeeded)
                        {
                            return Ok(user);
                        }
                        else
                        {
                            foreach (var item in userResult.Errors)
                            {
                                ModelState.AddModelError(item.Code, item.Description);
                            }
                        }
                    }

                }

                return BadRequest(ModelState.Values);
            }
            catch (Exception ex)
            {

                return StatusCode(500,"Internal Server error! Please cotact Admin");
            }
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        var user = await userManager.FindByNameAsync(model.UserName);
                        var roles = await userManager.GetRolesAsync(user);

                        //Step 1 Creating Claims

                        IdentityOptions identityOptions = new IdentityOptions();
                        var claims = new Claim[]
                        {
                            new Claim("userId",user.Id),
                            new Claim(identityOptions.ClaimsIdentity.UserIdClaimType,user.UserName),
                            new Claim(identityOptions.ClaimsIdentity.RoleClaimType,roles[0])

                        };

                        //step 2 : Creating signinKey from Secret key
                        var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));

                        //step 3 : Create signingCredentials from signinkey with HMAC algorithm
                         var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

                        //Step 4: Creating JW with signingCredentials, identityClaims & expire duration

                        var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, expires: DateTime.Now.AddSeconds(30), claims: claims);

                        //step 5: Write token as response with Ok()
                        return Ok( new
                        {
                            tokenJwt =new JwtSecurityTokenHandler().WriteToken(jwt),
                            id =user.Id,
                            userName=user.UserName,
                            role=roles[0]
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid userName or Password");
                    }
                }
                return BadRequest(ModelState.Values);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server error! Please cotact Admin");
            }
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut(SignInViewModel model)
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}