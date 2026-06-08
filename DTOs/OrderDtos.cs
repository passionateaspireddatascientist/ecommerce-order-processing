using System.ComponentModel.DataAnnotations;

namespace EcommerceOrderProcessing.Api.DTOs;

public record PlaceOrderDto(
    [Required] int UserId
);

public record UpdateOrderStatusDto(
    [Required, MaxLength(30)] string OrderStatus
);
