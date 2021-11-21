using SocialNinja.Contracts.Data;
using SocialNinja.Contracts.Data.Entities;
using SocialNinja.Contracts.Data.Repositories;
using SocialNinja.Migrations;

namespace SocialNinja.Core.Data.Repositories
{
    public class UserRepository : Repository<UserProfile>, IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetOrCreateExternalUserAsync(UserProfile id)
        {
            if (id != null)
            {
                var users = GetAll().Where(x => x.OId == id.OId && x.OIdProvider == id.OIdProvider);

                if (!users.Any())
                {
                    await _context.UserProfiles.AddAsync(id);
                    await _context.SaveChangesAsync();
                }
                return id.Id;
            }

            return -1;
        }
    }
}