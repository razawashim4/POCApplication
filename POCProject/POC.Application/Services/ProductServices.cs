using AutoMapper;
using POC.Application.DTO;
using POC.Application.IServices;
using POC.Domain.Entities;
using POC.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Application.Services
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductServices(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<APIResponse<ProductDto>> AddProductAsync(CreateProductDto productDto)
        {
            APIResponse<ProductDto> response = new APIResponse<ProductDto>();
            var product = _mapper.Map<Products>(productDto);
            var newProduct = await _repository.AddProductRepo(product);
            if (newProduct != null)
            {
                response.Message = "Successfully Created";
                response.StatusCode = 200;
                response.Data = _mapper.Map<ProductDto>(newProduct);
            }
            else
            {
                response.Message = "Something went wrong !";
                response.StatusCode = 500;
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse<List<ProductDto>>> GetAllProducts()
        {
            APIResponse<List<ProductDto>> response = new APIResponse<List<ProductDto>>();
            var newProduct = await _repository.GetAllProductsRepo();
            if (newProduct != null)
            {
                response.Message = "Data Found";
                response.StatusCode = 200;
                response.Data = _mapper.Map<List<ProductDto>>(newProduct);
            }
            else
            {
                response.Message = "Product Not Found !";
                response.StatusCode = 404;
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            APIResponse<ProductDto> response = new APIResponse<ProductDto>();
            var product = await _repository.GetProductsByIdRepo(id);
            if (product != null)
            {
                response.Message = "Data Found";
                response.StatusCode = 200;
                response.Data = _mapper.Map<ProductDto>(product);
            }
            else
            {
                response.Message = "Product Not Found !";
                response.StatusCode = 404;
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse<ProductDto>> DeleteProductById(int id)
        {
            APIResponse<ProductDto> response = new APIResponse<ProductDto>();
            var product = await _repository.DeleteProductRepo(id);
            if (product)
            {
                response.Message = "Successfully Deleted";
                response.StatusCode = 200;
                response.Data = null;
            }
            else
            {
                response.Message = "Product Not Found !";
                response.StatusCode = 404;
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse<UpdateProductDto>> UpdateProductAsync(UpdateProductDto model)
        {
            APIResponse<UpdateProductDto> response = new APIResponse<UpdateProductDto>();
            var data = _mapper.Map<Products>(model);
            var result = await _repository.UpdateProductRepo(data);
            response.Message= "Successfully Updated";
            response.StatusCode = 200;
            response.Data = _mapper.Map<UpdateProductDto>(result);
            return response;
        }
    }
}
