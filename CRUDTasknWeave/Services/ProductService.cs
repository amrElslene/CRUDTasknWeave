using CRUDTasknWeave.Data;
using CRUDTasknWeave.Models;
using CRUDTasknWeave.Serveces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDTasknWeave.Services
{
    public class ProductService : IProductsService
    {
        private readonly ApplicationDBContext _context;
        public ProductService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return  product;
        }
        public void UpdateProduct(Product updatedProduct)
        {
            _context.Update(updatedProduct);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product deletedProduct)
        {
            _context.Products.Remove(deletedProduct);
            _context.SaveChanges();
        }






    }
}
