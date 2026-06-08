using EcommerceOrderProcessing.Api.DTOs;
using EcommerceOrderProcessing.Api.Models;

namespace EcommerceOrderProcessing.Api.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> SearchAsync(string keyword);
    Task<Product> CreateAsync(ProductCreateDto dto);
    Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
