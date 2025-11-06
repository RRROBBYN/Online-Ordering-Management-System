using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class OrdersRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public OrdersRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // Get order by ID
        public async Task<Orders?> GetOrderByIdAsync(int orderId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Orders
                .Include(o => o.Status) // Include status details
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // Get order items with product details
        public async Task<List<OrderItems>> GetOrderItemsAsync(int orderId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.OrderItems
                .Include(oi => oi.Product) // Include product details
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        // Get order shipping address
        public async Task<UserShippingAddress?> GetOrderShippingAddressAsync(int orderId)
        {
            using var context = _contextFactory.CreateDbContext();
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order?.ShippingAddressId == null)
                return null;

            return await context.UserShippingAddress
                .FirstOrDefaultAsync(sa => sa.Id == order.ShippingAddressId);
        }

        // Get all orders for a user
        public async Task<List<Orders>> GetUserOrdersAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Orders
                .Include(o => o.Status) // Include status
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        // Create new order
        public async Task<int> CreateOrderAsync(Orders order)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order.Id;
        }

        // Add order items
        public async Task AddOrderItemsAsync(List<OrderItems> orderItems)
        {
            using var context = _contextFactory.CreateDbContext();
            context.OrderItems.AddRange(orderItems);
            await context.SaveChangesAsync();
        }

        // Update order status - FIXED: Use StatusId instead of Status
        public async Task UpdateOrderStatusAsync(int orderId, int statusId)
        {
            using var context = _contextFactory.CreateDbContext();
            var order = await context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.StatusId = statusId; // Changed from order.Status = status
                order.UpdatedAt = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        // Create order with shipping address and payment method
        public async Task<Orders> CreateOrderWithItemsAsync(
            string userId,
            List<OrderItems> items,
            int statusId,
            int? shippingAddressId = null,
            string? paymentMethod = null)
        {
            using var context = _contextFactory.CreateDbContext();

            // Calculate total
            var total = items.Sum(i => i.UnitPrice * i.Quantity);

            // Create new order
            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                StatusId = statusId, // Using FK to OrderStatus table
                ShippingAddressId = shippingAddressId,
                PaymentMethod = paymentMethod,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Save order first to get Id
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Attach OrderId to each order item
            foreach (var item in items)
            {
                item.OrderId = order.Id;
            }

            // Add all items
            context.OrderItems.AddRange(items);
            await context.SaveChangesAsync();

            return order;
        }
    }
}