using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

                        return Ok();
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


    }
}