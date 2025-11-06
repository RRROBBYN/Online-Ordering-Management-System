using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class ProductsRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ProductsRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // ➕ Add a new product
        public async Task AddProductAsync(Products product)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        // 📋 Get all products
        public async Task<List<Products>> GetAllProductsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        // 🔍 Get product by ID
        public async Task<Products?> GetProductByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products.FindAsync(id);
        }

        // ✏️ Update a product - FIXED VERSION
        public async Task UpdateProductAsync(Products product)
        {
            using var context = _contextFactory.CreateDbContext();

            // Find the existing product in the database
            var existing = await context.Products.FindAsync(product.Id);

            if (existing != null)
            {
                // Update only the properties we want to change
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.Unit = product.Unit;
                existing.CategoryId = product.CategoryId;
                existing.Description = product.Description;
                existing.ImageUrl = product.ImageUrl;

                // EF Core will track these changes automatically
                existing.UpdatedAt = DateTime.Now;
                await context.SaveChangesAsync();
            }
        }

        // ❌ Delete a product
        public async Task DeleteProductAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products.FindAsync(id);

            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
        }
    }
}