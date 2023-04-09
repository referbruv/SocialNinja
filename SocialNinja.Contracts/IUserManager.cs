using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Contracts
{
    public interface IUserManager
    {
        Task SignIn(string loginProvider, bool isPersistent = false);
        Task SignOut();
    }
}