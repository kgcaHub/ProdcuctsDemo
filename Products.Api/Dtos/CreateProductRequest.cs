using System.ComponentModel.DataAnnotations;

namespace Products.Api;

public record CreateProductRequest(
    [property: Required(ErrorMessage = "Name is required.")] string Name,
    [property: Required(ErrorMessage = "Category is required.")] string Category,
    [property: Range(0.01, double.MaxValue, ErrorMessage = "Price has to be more than 0")] decimal Price
);