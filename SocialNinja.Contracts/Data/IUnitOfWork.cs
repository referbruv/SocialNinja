using SocialNinja.Contracts.Data.Repositories;

namespace SocialNinja.Contracts.Data
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        INinjaRepository Ninjas { get; }
        void Commit();
    }
}