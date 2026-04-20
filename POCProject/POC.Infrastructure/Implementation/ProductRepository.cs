using Microsoft.EntityFrameworkCore;
using POC.Domain.Entities;
using POC.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Infrastructure.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Products?> AddProductRepo(Products product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Products>> GetAllProductsRepo()
        {
           return await _context.Products.OrderByDescending(x => x.Id).ToListAsync(); ;
        }

        public async Task<Products?> GetProductsByIdRepo(int id)
        {
            return await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteProductRepo(int id)
        {
            var product = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Products> UpdateProductRepo(Products products)
        {
            try
            {
                var existing = await _context.Products.FindAsync(products.Id);

                if (existing == null)
                    return null;

                // ✅ Update fields manually OR via AutoMapper
                existing.Name = products.Name;
                existing.Description = products.Description;
                existing.Price = products.Price;
                existing.Stock = products.Stock;

                await _context.SaveChangesAsync();

                return existing;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
