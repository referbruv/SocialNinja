using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OidcApp.Models.Entities;
using OidcApp.Models.Providers;

namespace OidcApp.Models.Repositories
{
    public interface IUserRepo
    {
        Task<bool> GetOrCreateExternalUserAsync(UserProfile id, HttpContext httpContext);
    }

    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;
        private readonly IUserManager userManager;

        public UserRepo(AppDbContext context, IUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> GetOrCreateExternalUserAsync(UserProfile id, HttpContext httpContext)
        {
            if (id != null)
            {
                UserProfile user = context.UserProfiles.FirstOrDefault(x => x.OId == id.OId && x.OIdProvider == id.OIdProvider);

                if (user == null)
                {
                    user = id;

                    await context.UserProfiles.AddAsync(user);

                    await context.SaveChangesAsync();
                }

                // await userManager.SignIn(httpContext, user);
                return true;
            }

            return false;
        }
    }
}