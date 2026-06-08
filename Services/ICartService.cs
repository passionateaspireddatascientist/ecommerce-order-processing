using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;

namespace EcommerceOrderProcessing.Api.Services;

public interface ICartService
{
    Task<List<CartItem>> GetByUserIdAsync(int userId);
    Task<CartItem> AddAsync(CartItemCreateDto dto);
    Task<bool> UpdateQuantityAsync(int cartItemId, CartItemUpdateDto dto);
    Task<bool> RemoveAsync(int cartItemId);
}
