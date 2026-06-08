using System.ComponentModel.DataAnnotations;

namespace EcommerceOrderProcessing.Api.DTOs;

public record CartItemCreateDto(
    [Required] int UserId,
    [Required] int ProductId,
    int Quantity
);

public record CartItemUpdateDto(
    int Quantity
);
