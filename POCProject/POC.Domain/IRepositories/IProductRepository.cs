using POC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<List<Products>> GetAllProductsRepo();
        Task<Products?> GetProductsByIdRepo(int id);
        Task<Products?> AddProductRepo(Products product);
        Task<Products> UpdateProductRepo(Products product);
        Task<bool> DeleteProductRepo(int id);
        //Task DeleteAsync(int id);
    }
}
