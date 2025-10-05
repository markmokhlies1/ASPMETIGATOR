using M05.ResponseCaching.Requests;
using M05.ResponseCaching.Responses;

namespace M05.ResponseCaching.Services;

public interface IProductService
{
    Task<PagedResult<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10);

    Task<ProductResponse?> GetProductByIdAsync(int productId);

    Task<ProductResponse> AddProductAsync(CreateProductRequest request);

    Task UpdateProductAsync(int productId, UpdateProductRequest request);

    Task DeleteProductAsync(int id);
}
