using EcommerceOrderProcessing.Api.Data;
using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrderProcessing.Api.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<List<Order>> GetByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order> PlaceOrderAsync(PlaceOrderDto dto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
        if (!userExists)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == dto.UserId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product is null || !cartItem.Product.IsActive)
            {
                throw new InvalidOperationException("Cart contains inactive or invalid product.");
            }

            if (cartItem.Product.StockQuantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product: {cartItem.Product.ProductName}");
            }
        }

        var order = new Order
        {
            UserId = dto.UserId,
            OrderStatus = "Confirmed",
            TotalAmount = cartItems.Sum(c => c.Quantity * c.Product!.Price)
        };

        foreach (var cartItem in cartItems)
        {
            cartItem.Product!.StockQuantity -= cartItem.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Product.Price
            });
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<bool> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto)
    {
        var allowedStatuses = new[] { "Pending", "Confirmed", "Shipped", "Delivered", "Cancelled" };

        if (!allowedStatuses.Contains(dto.OrderStatus, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid order status.");
        }

        var order = await _context.Orders.FindAsync(orderId);
        if (order is null)
        {
            return false;
        }

        order.OrderStatus = dto.OrderStatus;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order is null)
        {
            return false;
        }

        order.OrderStatus = "Cancelled";
        await _context.SaveChangesAsync();

        return true;
    }
}
