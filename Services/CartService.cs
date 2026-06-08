using EcommerceOrderProcessing.Api.Data;
using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrderProcessing.Api.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetByUserIdAsync(int userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<CartItem> AddAsync(CartItemCreateDto dto)
    {
        if (dto.Quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
        if (!userExists)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product is null || !product.IsActive)
        {
            throw new KeyNotFoundException("Product not found or inactive.");
        }

        if (product.StockQuantity < dto.Quantity)
        {
            throw new InvalidOperationException("Requested quantity exceeds available stock.");
        }

        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.ProductId == dto.ProductId);

        if (existingCartItem is not null)
        {
            existingCartItem.Quantity += dto.Quantity;
            await _context.SaveChangesAsync();
            return existingCartItem;
        }

        var cartItem = new CartItem
        {
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        return cartItem;
    }

    public async Task<bool> UpdateQuantityAsync(int cartItemId, CartItemUpdateDto dto)
    {
        if (dto.Quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem is null)
        {
            return false;
        }

        cartItem.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem is null)
        {
            return false;
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return true;
    }
}
