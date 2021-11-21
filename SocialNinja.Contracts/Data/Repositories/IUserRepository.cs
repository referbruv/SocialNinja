using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Contracts.Data.Repositories
{
    public interface IUserRepository : IRepository<UserProfile>
    {
        Task<int> GetOrCreateExternalUserAsync(UserProfile id);
    }
}