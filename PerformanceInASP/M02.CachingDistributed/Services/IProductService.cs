using M02.CachingInMemory.Requests;
using M02.CachingInMemory.Responses;

namespace M02.CachingInMemory.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();

    Task<ProductResponse?> GetProductByIdAsync(int productId);

    Task<ProductResponse> AddProductAsync(CreateProductRequest request);

    Task UpdateProductAsync(int productId, UpdateProductRequest request);

    Task DeleteProductAsync(int id);
}
