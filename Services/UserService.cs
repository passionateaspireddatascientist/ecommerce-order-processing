using EcommerceOrderProcessing.Api.Data;
using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceOrderProcessing.Api.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().OrderBy(u => u.FullName).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User> CreateAsync(UserCreateDto dto)
    {
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
        {
            throw new InvalidOperationException("User email already exists.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
