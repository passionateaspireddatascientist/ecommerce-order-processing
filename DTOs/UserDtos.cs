using System.ComponentModel.DataAnnotations;

namespace EcommerceOrderProcessing.Api.DTOs;

public record UserCreateDto(
    [Required, MaxLength(120)] string FullName,
    [Required, EmailAddress, MaxLength(150)] string Email,
    [Required, MaxLength(20)] string PhoneNumber,
    [MaxLength(250)] string? Address
);
