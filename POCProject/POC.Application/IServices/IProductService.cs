using POC.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Application.IServices
{
    public interface IProductService
    {
        Task<APIResponse<List<ProductDto>>> GetAllProducts();
        Task<APIResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<APIResponse<ProductDto>> AddProductAsync(CreateProductDto productDto);
        Task<APIResponse<UpdateProductDto>> UpdateProductAsync(UpdateProductDto model);
        Task<APIResponse<ProductDto>> DeleteProductById(int id);
        //Task DeleteProductAsync(int id);
    }
}
