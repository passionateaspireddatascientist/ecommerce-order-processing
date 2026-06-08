using System.ComponentModel.DataAnnotations;

namespace EcommerceOrderProcessing.Api.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required]
    [MaxLength(120)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;
}
