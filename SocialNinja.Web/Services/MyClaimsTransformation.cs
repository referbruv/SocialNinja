using Microsoft.AspNetCore.Authentication;
using SocialNinja.Contracts.Data;
using SocialNinja.Contracts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialNinja.Web.Services
{
    public class MyClaimsTransformation : IClaimsTransformation
    {
        private readonly IUnitOfWork _uow;

        public MyClaimsTransformation(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var claims = principal.Claims;
            var emailAddress = claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault();
            var oid = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var authProvider = principal.Identity.AuthenticationType;

            var loggedInUser = _uow.Users.GetQueryable().Where(
                x => x.EmailAddress == (emailAddress != null ? emailAddress.Value : "") &&
                x.OId == (oid != null ? oid.Value : "") &&
                x.OIdProvider == authProvider);

            if (loggedInUser.Any())
            {
                var user = loggedInUser.FirstOrDefault();
                AddLoggedInUserIdentity(principal, claims, user);
            }
            else
            {
                UserProfile userProfile = await AddNewUserToTheDatabase(principal);
                AddLoggedInUserIdentity(principal, claims, userProfile);
            }

            return principal;
        }

        private async Task<UserProfile> AddNewUserToTheDatabase(ClaimsPrincipal principal)
        {
            var id = principal.FindFirst(ClaimTypes.NameIdentifier);
            var email = principal.FindFirst(ClaimTypes.Email);
            var provider = principal.FindFirst("LoginProvider");

            var userProfile = new UserProfile
            {
                EmailAddress = email != null ? email.Value : "",
                OIdProvider = provider != null ? provider.Value : principal.Identity.AuthenticationType,
                OId = id != null ? id.Value : ""
            };

            var databaseId = await _uow.Users.GetOrCreateExternalUserAsync(userProfile);
            userProfile.Id = databaseId;
            return userProfile;
        }

        private void AddLoggedInUserIdentity(ClaimsPrincipal principal, IEnumerable<Claim> claims, UserProfile user)
        {
            var identity = new ClaimsIdentity();

            if (!principal.HasClaim(x => x.Type == "DatabaseId"))
            {
                identity.AddClaim(new Claim("DatabaseId", user.Id.ToString()));
            }

            if (!principal.HasClaim(x => x.Type == "LoginProvider"))
            {
                identity.AddClaim(new Claim("LoginProvider", user.OIdProvider.ToString()));
            }

            // Just to demonstrate
            // Add a special Role to all the Users
            // who have an EVEN database Identitifier
            if (user.Id % 2 == 0)
            {
                if (!principal.HasClaim(x => x.Type == "LoginProvider" && x.Value == "EvenUser"))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "EvenUser"));
                }
            }
            else
            {
                if (!principal.HasClaim(x => x.Type == "LoginProvider" && x.Value == "OddUser"))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "OddUser"));
                }
            }

            // add remaining claims from input
            foreach (var claim in claims)
            {
                if (!principal.HasClaim(x => x.Type == claim.Type))
                {
                    identity.AddClaim(claim);
                }
            }

            principal.AddIdentity(identity);
        }
    }
}
