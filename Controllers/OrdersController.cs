using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceOrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetByIdAsync(id);
        return order is null ? NotFound("Order not found.") : Ok(order);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        return Ok(await _service.GetByUserIdAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
    {
        try
        {
            var order = await _service.PlaceOrderAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(id, dto);
            return updated ? NoContent() : NotFound("Order not found.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await _service.CancelAsync(id);
        return cancelled ? NoContent() : NotFound("Order not found.");
    }
}
