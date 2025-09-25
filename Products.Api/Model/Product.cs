namespace Products.Api;
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Category { get; set; }
}