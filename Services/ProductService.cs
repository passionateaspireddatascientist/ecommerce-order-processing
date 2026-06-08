using EcommerceOrderProcessing.Api.Data;
using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrderProcessing.Api.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<List<Product>> SearchAsync(string keyword)
    {
        keyword = keyword.Trim().ToLower();

        return await _context.Products
            .AsNoTracking()
            .Where(p => p.ProductName.ToLower().Contains(keyword) || p.Category.ToLower().Contains(keyword))
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(ProductCreateDto dto)
    {
        if (dto.Price <= 0)
        {
            throw new InvalidOperationException("Price must be greater than zero.");
        }

        if (dto.StockQuantity < 0)
        {
            throw new InvalidOperationException("Stock quantity cannot be negative.");
        }

        var product = new Product
        {
            ProductName = dto.ProductName,
            Category = dto.Category,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            IsActive = dto.IsActive
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        product.ProductName = dto.ProductName;
        product.Category = dto.Category;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}
