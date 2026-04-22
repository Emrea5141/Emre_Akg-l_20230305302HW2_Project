using LibraryInventory.Data;
using LibraryInventory.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory.Business;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.StockQuantity = product.StockQuantity;
            existing.Price = product.Price;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    // Requirement: Must use LINQ (Advanced filtering and sorting)
    public async Task<List<Product>> GetFilteredInventoryAsync(string searchTerm)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // LINQ 'Where' clause for searching
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Category.Contains(searchTerm));
        }

        // LINQ 'OrderBy' for sorting by stock level
        return await query.OrderBy(p => p.StockQuantity).ToListAsync();
    }
}