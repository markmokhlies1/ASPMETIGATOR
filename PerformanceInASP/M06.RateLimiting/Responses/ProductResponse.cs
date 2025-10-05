using M06.RateLimiting.Models;

namespace M06.RateLimiting.Responses;

public class ProductResponse
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    private ProductResponse() { }

    public static ProductResponse FromModel(Product? product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product), "Cannot create a response from a null product");

        return new ProductResponse
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.Price,
        };
    }
}
