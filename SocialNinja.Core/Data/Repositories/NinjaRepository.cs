using SocialNinja.Contracts.Data.Entities;
using SocialNinja.Contracts.Data.Repositories;
using SocialNinja.Migrations;

namespace SocialNinja.Core.Data.Repositories
{
    public class NinjaRepository : Repository<Ninja>, INinjaRepository
    {
        public NinjaRepository(DatabaseContext context) : base(context)
        {
        }
    }
}