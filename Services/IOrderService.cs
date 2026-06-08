using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;

namespace EcommerceOrderProcessing.Api.Services;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByUserIdAsync(int userId);
    Task<Order> PlaceOrderAsync(PlaceOrderDto dto);
    Task<bool> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto);
    Task<bool> CancelAsync(int orderId);
}
