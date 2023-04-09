using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNinja.Contracts;
using SocialNinja.Contracts.Constants;
using SocialNinja.Contracts.Data;
using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Web.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _repo;
        private readonly IUserManager _manager;

        public UsersController(IUnitOfWork repo, IUserManager manager)
        {
            _repo = repo;
            _manager = manager;
        }

        [HttpGet, Route("[controller]/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        [HttpGet, Route("[controller]/Profile")]
        public IActionResult Profile()
        {
            var claims = User.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
            return View(claims);
        }

        [HttpGet, Route("[controller]/Logout")]
        public IActionResult Logout()
        {
            _manager.SignOut();
            return LocalRedirect("~/");
        }

        [HttpGet, Route("[controller]/ExternalLogin")]
        public IActionResult ExternalLogin(string returnUrl, string provider = "google")
        {
            string authenticationScheme = string.Empty;

            switch (provider)
            {
                case OidcProviderType.Facebook:
                    authenticationScheme = FacebookDefaults.AuthenticationScheme;
                    break;
                default:
                    authenticationScheme = GoogleDefaults.AuthenticationScheme;
                    break;
            }

            var auth = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(LoginCallback), new { provider, returnUrl })
            };

            return new ChallengeResult(authenticationScheme, auth);
        }

        [Authorize(Roles = "OddUser")]
        [HttpGet, Route("[controller]/OddUser")]
        public IActionResult OddUsersOnly()
        {
            return Ok("Hello there! You're an Odd User");
        }

        [Authorize(Roles = "EvenUser")]
        [HttpGet, Route("[controller]/EvenUser")]
        public IActionResult EvenUsersOnly()
        {
            return Ok("Hello there! You're an Even User");
        }

        public async Task<IActionResult> LoginCallback(string provider, string returnUrl = "~/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(SocialAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest();

            //var id = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
            //var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            //var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name);

            //var obj = new UserProfile
            //{
            //    EmailAddress = email != null ? email.Value : "",
            //    OIdProvider = provider,
            //    OId = id != null ? id.Value : ""
            //};

            //var userId = await _repo.Users.GetOrCreateExternalUserAsync(obj);
            //obj.Id = userId;

            await _manager.SignIn(provider);

            return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl);
        }
    }
}