using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class ShippingAddressRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ShippingAddressRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Get all addresses for a user
        public async Task<List<UserShippingAddress>> GetUserAddressesAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.UserShippingAddress
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.UpdatedAt)
                .ToListAsync();
        }

        // Get default address for a user
        public async Task<UserShippingAddress?> GetDefaultAddressAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.UserShippingAddress
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }

        // Save or update address
        public async Task<UserShippingAddress> SaveAddressAsync(UserShippingAddress address)
        {
            using var context = _contextFactory.CreateDbContext();

            // If this is set as default, unset all other defaults for this user
            if (address.IsDefault)
            {
                var otherAddresses = await context.UserShippingAddress
                    .Where(a => a.UserId == address.UserId && a.Id != address.Id)
                    .ToListAsync();

                foreach (var addr in otherAddresses)
                {
                    addr.IsDefault = false;
                }
            }

            if (address.Id == 0)
            {
                // New address
                context.UserShippingAddress.Add(address);
            }
            else
            {
                // Update existing
                address.UpdatedAt = DateTime.Now;
                context.UserShippingAddress.Update(address);
            }

            await context.SaveChangesAsync();
            return address;
        }

        // Delete address
        public async Task DeleteAddressAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var address = await context.UserShippingAddress.FindAsync(id);

            if (address != null)
            {
                context.UserShippingAddress.Remove(address);
                await context.SaveChangesAsync();
            }
        }
    }
}