using CRUDTasknWeave.Data;
using CRUDTasknWeave.Models;
using CRUDTasknWeave.Serveces;
using Microsoft.AspNetCore.Mvc;

namespace CRUDTasknWeave.Services
{
    public class ProductService : IProductsService
    {
        private readonly ApplicationDBContext _context;
        public ProductService(ApplicationDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        public Product CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
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
