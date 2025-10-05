using M07.ResponseCompression.Requests;
using M07.ResponseCompression.Responses;

namespace M07.ResponseCompression.Services;

public interface IProductService
{
    Task<PagedResult<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10);

    Task<ProductResponse?> GetProductByIdAsync(int productId);

    Task<ProductResponse> AddProductAsync(CreateProductRequest request);

    Task UpdateProductAsync(int productId, UpdateProductRequest request);

    Task DeleteProductAsync(int id);
}
