using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class UserRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public UserRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserAccount?> GetUserByEmailAsync(string email)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.UserAccount.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(UserAccount user)
        {
            using var context = _contextFactory.CreateDbContext();
            context.UserAccount.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = await context.UserAccount.FirstOrDefaultAsync(u => u.Email == email);
            return user != null && user.Password == password;
        }
    }
}
    