using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Contracts
{
    public interface IUserManager
    {
        Task SignIn(UserProfile user, bool isPersistent = false);
        Task SignOut();
    }
}