using _4IdiotsInc.Data;
using _4IdiotsInc.Model;
using Microsoft.EntityFrameworkCore;

namespace _4IdiotsInc.Repositories
{
    public class ProductCategoryRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ProductCategoryRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // ➕ Add new category
        public async Task AddCategoryAsync(ProductCategory category)
        {
            using var context = _contextFactory.CreateDbContext();
            context.ProductCategory.Add(category);
            await context.SaveChangesAsync();
        }

        // 📋 Get all categories
        public async Task<List<ProductCategory>> GetAllCategoriesAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.ProductCategory
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        // 🔍 Get category by ID
        public async Task<ProductCategory?> GetCategoryByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.ProductCategory.FindAsync(id);
        }

        // ✏️ Update category
        public async Task UpdateCategoryAsync(ProductCategory category)
        {
            using var context = _contextFactory.CreateDbContext();
            context.ProductCategory.Update(category);
            await context.SaveChangesAsync();
        }

        // ❌ Delete category
        public async Task DeleteCategoryAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var category = await context.ProductCategory.FindAsync(id);
            if (category != null)
            {
                context.ProductCategory.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }
}
