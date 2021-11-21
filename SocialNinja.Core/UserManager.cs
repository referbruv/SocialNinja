using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SocialNinja.Contracts;
using SocialNinja.Contracts.Constants;
using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Web.Models.Providers
{
    public class UserManager : IUserManager
    {
        private readonly HttpContext _context;

        public UserManager(IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor.HttpContext;
        }

        public async Task SignIn(UserProfile user, bool isPersistent = false)
        {
            string authenticationScheme = SocialAuthenticationDefaults.AuthenticationScheme;

            // Generate Claims from DbEntity
            var claims = GetUserClaims(user);

            // Add Additional Claims from the Context
            // which might be useful
            var contextClaims = _context.User.Claims.ToList();
            contextClaims.AddRange(claims);
            
            ClaimsIdentity claimsIdentity = new(contextClaims, authenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. Required when setting the 
                // ExpireTimeSpan option of CookieAuthenticationOptions 
                // set with AddCookie. Also required when setting 
                // ExpiresUtc.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await _context.SignInAsync(authenticationScheme, claimsPrincipal, authProperties);
        }

        public async Task SignOut()
        {
            // await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _context.SignOutAsync(SocialAuthenticationDefaults.AuthenticationScheme);
        }

        private static List<Claim> GetUserClaims(UserProfile user)
        {
            List<Claim> claims = new()
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Provider", user.OIdProvider)
            };
            return claims;
        }
    }
}