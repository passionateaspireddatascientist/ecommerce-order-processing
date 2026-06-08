using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceOrderProcessing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        return user is null ? NotFound("User not found.") : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        try
        {
            var user = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
