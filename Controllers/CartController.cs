using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceOrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        return Ok(await _service.GetByUserIdAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> Add(CartItemCreateDto dto)
    {
        try
        {
            var cartItem = await _service.AddAsync(dto);
            return Ok(cartItem);
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

    [HttpPut("{cartItemId:int}")]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, CartItemUpdateDto dto)
    {
        try
        {
            var updated = await _service.UpdateQuantityAsync(cartItemId, dto);
            return updated ? NoContent() : NotFound("Cart item not found.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{cartItemId:int}")]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        var removed = await _service.RemoveAsync(cartItemId);
        return removed ? NoContent() : NotFound("Cart item not found.");
    }
}
