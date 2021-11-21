using SocialNinja.Contracts.Data;
using SocialNinja.Contracts.Data.Repositories;
using SocialNinja.Core.Data.Repositories;
using SocialNinja.Migrations;

namespace SocialNinja.Core.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IUserRepository Users => new UserRepository(_context);
        public INinjaRepository Ninjas => new NinjaRepository(_context);

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}