using System.ComponentModel.DataAnnotations;

namespace EcommerceOrderProcessing.Api.DTOs;

public record ProductCreateDto(
    [Required, MaxLength(120)] string ProductName,
    [Required, MaxLength(80)] string Category,
    decimal Price,
    int StockQuantity,
    bool IsActive
);

public record ProductUpdateDto(
    [Required, MaxLength(120)] string ProductName,
    [Required, MaxLength(80)] string Category,
    decimal Price,
    int StockQuantity,
    bool IsActive
);
