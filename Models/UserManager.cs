using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OidcApp.Models.Entities;

namespace OidcApp.Models.Providers
{
    public interface IUserManager
    {
        Task SignIn(HttpContext httpContext, UserProfile user, bool isPersistent = false);
        Task SignOut(HttpContext httpContext);
    }

    public class UserManager : IUserManager
    {
        public async Task SignIn(HttpContext httpContext, UserProfile user, bool isPersistent = false)
        {
            string authenticationScheme = SocialAuthenticationDefaults.AuthenticationScheme;

            // Generate Claims from DbEntity
            var claims = GetUserClaims(user);
            
            // Add Additional Claims from the Context
            // which might be useful
            claims.Add(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name));
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
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

            await httpContext.SignInAsync(authenticationScheme, claimsPrincipal, authProperties);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            // await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContext.SignOutAsync(SocialAuthenticationDefaults.AuthenticationScheme);
        }

        private List<Claim> GetUserClaims(UserProfile user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim("Provider", user.OIdProvider));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
            return claims;
        }
    }
}