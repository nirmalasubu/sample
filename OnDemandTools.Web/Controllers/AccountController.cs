using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.User;

namespace OnDemandTools.Web.Controllers
{
     
    public class AccountController : Controller
    {
        IUserHelper userHelper;

        public AccountController(IUserHelper userHelper)
        {
            this.userHelper = userHelper;
        }


        // GET: /Account/Login
        [HttpGet]
        public async Task Login()
        {
            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.Authentication.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { });
        }

       
        [Authorize]
        // GET: /Account/LogOff
        [HttpGet]

        public async Task LogOff()
        {
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }          
            
        }
    
        [Authorize]
        [HttpGet]
        public async Task EndSession()
        {
            // If AAD sends a single sign-out message to the app, end the user's session, but don't redirect to AAD for sign out.
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        // GET: /Account/Access
        [HttpGet]

        public IActionResult Access()
        {
            return Json("Good, you have access!");            
        }

         [Authorize]
        // GET: /Account/AccessDenied
        [HttpGet]

        public IActionResult AccessDenied()
        {
            return Json("Sorry, your access is denied");            
        }
    }
}
