using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class OrderStatusRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public OrderStatusRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // 📋 Get all order statuses
        public async Task<List<OrderStatus>> GetAllStatusesAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.OrderStatus.ToListAsync();
        }

        // 🔍 Get status by ID
        public async Task<OrderStatus?> GetStatusByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.OrderStatus.FindAsync(id);
        }

        // 🔍 Get status by name
        public async Task<OrderStatus?> GetStatusByNameAsync(string statusName)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.OrderStatus
                .FirstOrDefaultAsync(s => s.StatusName == statusName);
        }

        // ➕ Add new status
        public async Task AddStatusAsync(OrderStatus status)
        {
            using var context = _contextFactory.CreateDbContext();
            context.OrderStatus.Add(status);
            await context.SaveChangesAsync();
        }

        // ✏️ Update status
        public async Task UpdateStatusAsync(OrderStatus status)
        {
            using var context = _contextFactory.CreateDbContext();
            var existing = await context.OrderStatus.FindAsync(status.Id);

            if (existing != null)
            {
                existing.StatusName = status.StatusName;
                await context.SaveChangesAsync();
            }
        }

        // ❌ Delete status
        public async Task DeleteStatusAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var status = await context.OrderStatus.FindAsync(id);

            if (status != null)
            {
                context.OrderStatus.Remove(status);
                await context.SaveChangesAsync();
            }
        }
    }
}