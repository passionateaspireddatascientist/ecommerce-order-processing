using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;

namespace EcommerceOrderProcessing.Api.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(UserCreateDto dto);
}
